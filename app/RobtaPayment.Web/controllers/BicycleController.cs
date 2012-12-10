using System;
using System.Linq;

namespace RobtaPayment.Web.controllers
{
    using System.Configuration;
    using System.Net.Mail;
    using Castle.ActiveRecord;
    using Castle.MonoRail.ActiveRecordSupport;
    using Model.Entities;
    using Model.Enums;
    using Model.Helpers;

    public class BicycleController : ControllerBase
    {
        protected readonly string mollieClientNumber;
        protected readonly bool mollieTestMode;

        public BicycleController()
        {
            mollieClientNumber = ConfigurationManager.AppSettings["MollieClientNumber"];
            mollieTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["MollieTestMode"]);
        }

        public void Index()
        {
            PropertyBag.Add("rack", BicycleRack.FindFirst());
        }

        public void Enrol()
        {
            var bicycleRack = BicycleRack.FindFirst();
            PropertyBag.Add("rack", bicycleRack);
            if (bicycleRack.IsOpen)
            {
                PropertyBag.Add("enrolment", new BicycleRackEnrolment(bicycleRack));
            }
            else
            {
                PropertyBag.Add("error", "Alle plekken in het fietsenrek zijn uitgegeven.");
            }
        }

        public void Payment([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id, Guid")] BicycleRackEnrolment enrolment, [ARFetch("rack", false, true)] BicycleRack rack)
        {
            enrolment.BicycleRack = rack;
            if(!enrolment.BicycleRack.IsOpen)
            {
                Flash["error"] = "Het fietsenrek is helaas volgeboekt.";
                RedirectToAction("index");
                return;
            }
            if(!enrolment.IsValid())
            {
                Flash["error"] = "Je hebt niet alle velden ingevuld, vul alle velden in en probeer het opnieuw.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("rack", enrolment.BicycleRack);
                RenderView("enrol");
                return;
            }
            if (BicycleRackEnrolment.FindAllByProperty("StudentNumber", enrolment.StudentNumber).Any(e => e.Status != BicycleRackEnrolmentStatus.EnrolmentFinished))
            {
                Flash["error"] = "Je bent al ingeschreven voor een plek in het fietsenrek, je kunt maar aan 1 plek boeken.";
                RedirectToAction("index");
                return;
            }
            Transaction transaction = new Transaction();
            transaction.Amount = Convert.ToDecimal(ConfigurationManager.AppSettings["BicycleRackPrice"]);
            transaction.Save();
            enrolment.Transaction = transaction;
            enrolment.SaveAndFlush();

            PropertyBag.Add("banks", MollieIdealHelper.GetIdealBanks(mollieClientNumber, mollieTestMode));
            PropertyBag.Add("transaction", enrolment.Transaction);
        }

    }
}