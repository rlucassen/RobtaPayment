namespace RobtaPayment.Web.controllers.admin
{
    #region

    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Entities;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Web.helpers;

    #endregion

    [ControllerDetails(Area = "Admin")]
    public class ContributionEnrolmentsController : SecureController
    {
        public void Index([ARFetch("contribution", false, false)] Contribution contribution)
        {
            if (contribution == null)
            {
                PropertyBag.Add("enrolments", ContributionEnrolment.FindAll());
            }
            else
            {
                PropertyBag.Add("enrolments", ContributionEnrolment.FindAllByProperty("Contribution", contribution));
                PropertyBag.Add("contribution", contribution);
            }
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("enrolment", new ContributionEnrolment());
            PropertyBag.Add("contributions", Contribution.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] ContributionEnrolment enrolment)
        {
            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("contributions", Contribution.FindAll());
                RenderView("new");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");
        }

        public void SetPayed([ARFetch("id", false, true)] ContributionEnrolment enrolment)
        {
            Transaction transaction = new Transaction {Amount = enrolment.Contribution.Price, Payed = true};
            enrolment.Transaction = transaction;
            transaction.SaveAndFlush();
            enrolment.SaveAndFlush();

            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("contributions", Contribution.FindAll());
            RenderView("edit");
        }

        public void Edit([ARFetch("id", false, true)] ContributionEnrolment enrolment)
        {
            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("contributions", Contribution.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] ContributionEnrolment enrolment)
        {
            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("contributions", Contribution.FindAll());
                RedirectToAction("edit");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] ContributionEnrolment enrolment)
        {
            enrolment.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}