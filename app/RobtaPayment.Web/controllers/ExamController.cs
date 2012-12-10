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

    public class ExamController : ControllerBase
    {
        protected readonly string mollieClientNumber;
        protected readonly bool mollieTestMode;

        public ExamController()
        {
            mollieClientNumber = ConfigurationManager.AppSettings["MollieClientNumber"];
            mollieTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["MollieTestMode"]);
        }

        public void Index()
        {
            PropertyBag.Add("exams", Exam.FindAll().Where(e => e.IsOpen).ToList());
            PropertyBag.Add("educations", Education.FindAll());
            PropertyBag.Add("enrolment", new ExamEnrolment());
        }

        public void Payment([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewRootInstanceIfInvalidKey, Exclude = "Id, Guid")] ExamEnrolment enrolment)
        {
            if(enrolment.StudentNumber == null)
            {
                enrolment.StudentNumber = "0";
            }
            if(!enrolment.Exam.IsOpen)
            {
                Flash["error"] = "Deze toets is verlopen.";
                RedirectToAction("index");
                return;
            }
            if(!enrolment.IsValid())
            {
                Flash["error"] = "Je hebt niet alle velden ingevuld, vul alle velden in en probeer het opnieuw.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("exam", enrolment.Exam);
                PropertyBag.Add("exams", Exam.FindAll().Where(e => e.IsOpen).ToList());
                PropertyBag.Add("educations", Education.FindAll());
                RenderView("index");
                return;
            }
            Transaction transaction = new Transaction();
            transaction.Amount = (decimal)enrolment.Exam.Price;
            transaction.Save();
            enrolment.Transaction = transaction;
            enrolment.SaveAndFlush();

            PropertyBag.Add("banks", MollieIdealHelper.GetIdealBanks(mollieClientNumber, mollieTestMode));
            PropertyBag.Add("transaction", enrolment.Transaction);
        }

    }
}