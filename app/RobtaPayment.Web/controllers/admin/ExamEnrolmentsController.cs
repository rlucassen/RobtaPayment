using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobtaPayment.Web.controllers.admin
{
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Web.filters;
    using RobtaPayment.Web.helpers;
    using Model.Entities;

    [ControllerDetails(Area = "Admin")]
    public class ExamEnrolmentsController : SecureController
    {
        public void Index([ARFetch("exam", false, false)] Exam exam)
        {
            if (exam == null)
            {
                PropertyBag.Add("enrolments", ExamEnrolment.FindAll());
            }
            else
            {
                PropertyBag.Add("enrolments", ExamEnrolment.FindAllByProperty("Exam", exam));
                PropertyBag.Add("exam", exam);
            }

        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("enrolment", new ExamEnrolment());
            PropertyBag.Add("exams", Exam.FindAll());
            PropertyBag.Add("educations", Education.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] ExamEnrolment enrolment)
        {

            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("exams", Exam.FindAll());
                PropertyBag.Add("educations", Education.FindAll());
                RenderView("new");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");

        }

        public void SetPayed([ARFetch("id", false, true)] ExamEnrolment enrolment)
        {
            Transaction transaction = new Transaction { Amount = (decimal)enrolment.Exam.Price, Payed = true };
            enrolment.Transaction = transaction;
            transaction.SaveAndFlush();
            enrolment.SaveAndFlush();

            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("exams", Exam.FindAll());
            PropertyBag.Add("educations", Education.FindAll());
            RenderView("edit");
        }

        public void Edit([ARFetch("id", false, true)] ExamEnrolment enrolment)
        {
            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("exams", Exam.FindAll());
            PropertyBag.Add("educations", Education.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] ExamEnrolment enrolment)
        {
            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("exams", Exam.FindAll());
                PropertyBag.Add("educations", Education.FindAll());
                RedirectToAction("edit");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] ExamEnrolment enrolment)
        {
            enrolment.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}