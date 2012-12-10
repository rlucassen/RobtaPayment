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

    public class ActivityController : ControllerBase
    {
        protected readonly string mollieClientNumber;
        protected readonly bool mollieTestMode;

        public ActivityController()
        {
            mollieClientNumber = ConfigurationManager.AppSettings["MollieClientNumber"];
            mollieTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["MollieTestMode"]);
        }

        public void Index()
        {
            PropertyBag.Add("activities", Activity.FindAll());
        }

        public void Enrol([ARFetch("activity", false, true)] Activity activity)
        {
            PropertyBag.Add("activity", activity);
            if (activity.IsOpen)
            {
                PropertyBag.Add("enrolment", new ActivityEnrolment(activity));
            }
            else
            {
                PropertyBag.Add("error", "Deze activiteit is al volgeboekt of verlopen");
            }
        }

        public void Payment([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id, Guid")] ActivityEnrolment enrolment, [ARFetch("activity", false, true)] Activity activity)
        {
            enrolment.Activity = activity;
            if(!enrolment.Activity.IsOpen)
            {
                Flash["error"] = "Deze activiteit is volgeboekt of verlopen, kies een andere activiteit.";
                RedirectToAction("index");
                return;
            }
            if(!enrolment.IsValid())
            {
                Flash["error"] = "Je hebt niet alle velden ingevuld, vul alle velden in en probeer het opnieuw.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("activity", enrolment.Activity);
                RenderView("enrol");
                return;
            }
            if (ActivityEnrolment.FindAllByProperty("StudentNumber", enrolment.StudentNumber).Any())
            {
                Flash["error"] = "Je bent al ingeschreven voor een andere activiteit, je kunt maar aan 1 activiteit deelnemen";
                RedirectToAction("index");
                return;
            }
            Transaction transaction = new Transaction();
            transaction.Amount = (decimal)enrolment.Activity.DownPaymentPrice;
            transaction.Save();
            enrolment.Transaction = transaction;
            enrolment.SaveAndFlush();

            PropertyBag.Add("banks", MollieIdealHelper.GetIdealBanks(mollieClientNumber, mollieTestMode));
            PropertyBag.Add("transaction", enrolment.Transaction);
        }

    }
}