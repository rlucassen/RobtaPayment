using System;
using System.Linq;

namespace RobtaPayment.Web.controllers
{
    using System.Configuration;
    using System.Net.Mail;
    using Castle.ActiveRecord;
    using Castle.MonoRail.ActiveRecordSupport;
    using Model.Entities;
    using Model.Helpers;

    public class ContributionController : ControllerBase
    {
        protected readonly string mollieClientNumber;
        protected readonly bool mollieTestMode;

        public ContributionController()
        {
            mollieClientNumber = ConfigurationManager.AppSettings["MollieClientNumber"];
            mollieTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["MollieTestMode"]);
        }

        public void Index()
        {
            PropertyBag.Add("contributions", Contribution.FindAll());
        }

        public void Payment([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id, Guid")] ContributionEnrolment enrolment)
        {
            if(!enrolment.IsValid())
            {
                Flash["error"] = "Je hebt niet alle velden ingevuld, vul alle velden in en probeer het opnieuw.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("contributions", Contribution.FindAll());
                RenderView("index");
                return;
            }
            if (ContributionEnrolment.FindAllByProperty("StudentNumber", enrolment.StudentNumber).Any())
            {
                Flash["error"] = "Je hebt de schoolbijdrage al betaald, je kunt maar 1 keer een schoolbijdrage betalen.";
                RedirectToAction("index");
                return;
            }
            Transaction transaction = new Transaction();
            transaction.Amount = enrolment.Contribution.Price;
            transaction.Save();
            enrolment.Transaction = transaction;
            enrolment.SaveAndFlush();

            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("banks", MollieIdealHelper.GetIdealBanks(mollieClientNumber, mollieTestMode));
            PropertyBag.Add("transaction", enrolment.Transaction);
        }

    }
}