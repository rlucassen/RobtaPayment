namespace RobtaPayment.Web.controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using Castle.ActiveRecord;
    using Castle.MonoRail.ActiveRecordSupport;
    using Model.Entities;
    using Model.Helpers;
    using NHibernate.Criterion;

    public class IdealController : ControllerBase
    {
        protected readonly string mollieClientNumber;
        protected readonly bool mollieTestMode;
        protected readonly string molliePaymentName;
        protected readonly bool mollieUseTestUrls;

        public IdealController()
        {
            mollieClientNumber = ConfigurationManager.AppSettings["MollieClientNumber"];
            mollieTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["MollieTestMode"]);
            molliePaymentName = ConfigurationManager.AppSettings["MolliePaymentName"];
            mollieUseTestUrls = Convert.ToBoolean(ConfigurationManager.AppSettings["MollieUseTestUrls"]);
        }

        public void Pay([ARDataBind("transaction", AutoLoadBehavior.NullIfInvalidKey)] Transaction transaction)
        {
            Uri uri = Request.Uri;

            string siteUrl = uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.IndexOf(uri.AbsolutePath));

            string reportUrl = string.Format("{0}/Ideal/MollieCallback", siteUrl);
            string landingUrl = string.Format("{0}/Ideal/Landing", siteUrl);

            if (mollieUseTestUrls)
            {
                reportUrl = ConfigurationManager.AppSettings["MollieTestCallback"];
                landingUrl = ConfigurationManager.AppSettings["MollieTestLanding"];
            }

            using (TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
            {
                transaction.Save();
                transactionScope.VoteCommit();
            }

            var enrolment = GetEnrolmentByTransaction(transaction);
            string molliePaymentDiscription = GetMollieDescription(enrolment);

            RedirectToUrl(MollieIdealHelper.GetPaymentUrl(transaction, mollieClientNumber, mollieTestMode, molliePaymentDiscription,
                                                                 reportUrl, landingUrl));
        }

        private string GetMollieDescription(IEnrolment enrolment)
        {
            if(enrolment is ExamEnrolment)
            {
                ExamEnrolment examEnrolment = (ExamEnrolment) enrolment;
                if (examEnrolment.StudentNumber != "0")
                {
                    return string.Format("{0}/{1}/{2}", examEnrolment.Exam.Name, examEnrolment.StudentNumber,
                                         examEnrolment.FullName);
                }
                return string.Format("{0}/{1}", examEnrolment.Exam.Name, examEnrolment.FullName);
            }
            if(enrolment is LockerEnrolment)
            {
                LockerEnrolment lockerEnrolment = (LockerEnrolment) enrolment;
                return string.Format("{0}/{1}/{2}", lockerEnrolment.StudentNumber, lockerEnrolment.Locker.Number,
                                     enrolment.Name);
            }
            if(enrolment is BicycleRackEnrolment)
            {
                return string.Format("{0}/{1}", enrolment.StudentNumber, enrolment.Name);
            }
            if(enrolment is ActivityEnrolment)
            {
                ActivityEnrolment activityEnrolment = (ActivityEnrolment) enrolment;
                return string.Format("{0}/{1}/{2}", activityEnrolment.StudentNumber,
                                     activityEnrolment.Activity.Name.Substring(0, 10), activityEnrolment.Name);
            }
            return string.Format("{0}/onbekend/{1}", enrolment.StudentNumber, enrolment.Name);
        }

        public void MollieCallback(int id, string transaction_id)
        {
            Transaction transaction = MollieIdealHelper.GetTransactionByTransactionId(transaction_id);

            var enrolment = GetEnrolmentByTransaction(transaction);
            if (!enrolment.Active)
            {
                if (enrolment.CanBeMadeActive())
                {
                    enrolment.Active = true;
                    ActiveRecordMediator.SaveAndFlush(enrolment);
                }
                else
                {
                    SendMailForInactiveEnrolment(enrolment);
                    return;
                }
            }

            transaction.Payed = MollieIdealHelper.IsTransactionPayed(transaction_id, mollieClientNumber, mollieTestMode);

            using (TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
            {
                transaction.Save();
                transactionScope.VoteCommit();
            }

            if (transaction.Payed)
            {
                SendMail(transaction);
            }

            CancelView();
        }

        private void SendMailForInactiveEnrolment(IEnrolment enrolment)
        {
            try
            {
                PropertyBag.Add("enrolment", enrolment);
                
                MailMessage message = RenderMailMessage("inactiveenrolment", null, PropertyBag);
                message.Subject = ConfigurationManager.AppSettings["RefundingSubject"];
                message.To.Add(new MailAddress(ConfigurationManager.AppSettings["RefundingAddress"]));
                message.From = new MailAddress(ConfigurationManager.AppSettings["ConfirmationFromAddress"]);

                DeliverEmail(message);
            }
            catch (Exception e)
            {

            }
        }

        public void InvoiceExample(int id, string transaction_id)
        {
            Transaction transaction = MollieIdealHelper.GetTransactionByTransactionId(transaction_id);
            var enrolment = GetEnrolmentByTransaction(transaction);

            PropertyBag.Add("transaction", transaction);
            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("cssFile", Path.Combine(HttpContext.Server.MapPath(@"\content\styles"), "style.css"));
            CancelLayout();
        }

        public void Landing(int id, string transaction_id)
        {
            Transaction transaction = MollieIdealHelper.GetTransactionByTransactionId(transaction_id);

            if(transaction == null)
            {
                RenderView("toolate");
                return;
            }

            var enrolment = GetEnrolmentByTransaction(transaction);

            if (!enrolment.Active && !enrolment.CanBeMadeActive())
            {
                RenderView("toolate");
                return;
            }

            PropertyBag.Add("enrolment", enrolment);

            if (transaction.Payed)
                RenderView("success");
            else
                RenderView("error");
        }

        private void SendMail(Transaction transaction)
        {
            var enrolment = GetEnrolmentByTransaction(transaction);
            try
            {
                PropertyBag.Add("transaction", transaction);
                PropertyBag.Add("enrolment", enrolment);

                MailMessage message = RenderMailMessage(string.Format("confirmation_{0}", enrolment.GetType().Name), null, PropertyBag);
                message.Subject = ConfigurationManager.AppSettings["ConfirmationSubject"];
                message.To.Add(new MailAddress(enrolment.Email));
                message.From = new MailAddress(ConfigurationManager.AppSettings["ConfirmationFromAddress"]);

                if (enrolment.GetType() == typeof(LockerEnrolment))
                {
                    message.CC.Add(new MailAddress(ConfigurationManager.AppSettings["RobtaPaymentAdministrationEmail"]));
                }
                if (enrolment.GetType() == typeof(ContributionEnrolment))
                {
                    Dictionary<string, object> objects = new Dictionary<string, object>();

                    objects.Add("transaction", transaction);
                    objects.Add("enrolment", enrolment);
                    objects.Add("cssFile", Path.Combine(HttpContext.Server.MapPath(@"\content\styles"), "style.css"));

                    message.Attachments.Add(
                        new Attachment(
                            InvoiceHelper.GetInvoice("ContributionInvoice",
                                                     objects),
                            "test.pdf"));
                }

                DeliverEmail(message);
            }
            catch (Exception e)
            {

            }

        }

        private IEnrolment GetEnrolmentByTransaction(Transaction transaction)
        {
            var session =
                ActiveRecordMediator.GetSessionFactoryHolder().GetSessionFactory(typeof (Enrolment<>)).OpenSession();
            var query = session.CreateQuery("from IEnrolment where Transaction = :transaction")
                .SetParameter("transaction", transaction);

            var enrolments = query.List<IEnrolment>();
            return enrolments.First();
        }
    }
}