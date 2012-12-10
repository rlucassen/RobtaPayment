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
    using Model.Helpers;

    [ControllerDetails(Area = "Admin")]
    public class ExamsController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("exams", Exam.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("exam", new Exam());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("exam", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] Exam exam, string expirationTime)
        {
            exam.SetExpirationTime(expirationTime);

            if(!exam.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("exam", exam);
                RenderView("new");
                return;
            }

            exam.SaveAndFlush();
            RedirectToAction("index");

        }

        public void Edit([ARFetch("id", false, true)] Exam exam)
        {
            PropertyBag.Add("exam", exam);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("exam", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] Exam exam, string expirationTime, string openingTime)
        {
            exam.SetOpeningTime(openingTime);
            exam.SetExpirationTime(expirationTime);

            if (!exam.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("exam", exam);
                RedirectToAction("edit", new { Id = exam.Id});
                return;
            }

            exam.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] Exam exam)
        {
            exam.DeleteAndFlush();
            RedirectToAction("index");
        }

        public void Csv([ARFetch("id")] Exam exam)
        {
            var csvString = CsvHelper.GenerateCsvForExamEnrolments(exam);
            var filename = string.Format("inschrijvingen-toets-{0}.csv", exam.Name);

            CancelView();
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Write(csvString);
        }

        public void Presence([ARFetch("id")] Exam exam)
        {
            PropertyBag.Add("exam", exam);
            PropertyBag.Add("enrolments", exam.Enrolments);
        }

        public void UpdatePresence([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] ExamEnrolment[] enrolments, [ARFetch("exam.Id")] Exam exam)
        {
            foreach (var enrolment in enrolments)
            {
                enrolment.SaveAndFlush();
            }
            RedirectToAction("presence", new { Id = exam.Id });
        }
    }
}