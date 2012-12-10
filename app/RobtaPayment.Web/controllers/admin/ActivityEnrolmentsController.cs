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
    public class ActivityEnrolmentsController : SecureController
    {
        public void Index([ARFetch("activity", false, false)] Activity activity)
        {
            if (activity == null)
            {
                PropertyBag.Add("enrolments", ActivityEnrolment.FindAll());
            }
            else
            {
                PropertyBag.Add("enrolments", ActivityEnrolment.FindAllByProperty("Activity", activity));
                PropertyBag.Add("activity", activity);
            }

        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("enrolment", new ActivityEnrolment());
            PropertyBag.Add("activities", Activity.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] ActivityEnrolment enrolment)
        {

            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("activities", Activity.FindAll());
                RenderView("new");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");

        }

        public void SetPayed([ARFetch("id", false, true)] ActivityEnrolment enrolment)
        {
            Transaction transaction = new Transaction { Amount = (decimal)enrolment.Activity.DownPaymentPrice, Payed = true };
            enrolment.Transaction = transaction;
            transaction.SaveAndFlush();
            enrolment.SaveAndFlush();

            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("activities", Activity.FindAll());
            RenderView("edit");
        }

        public void Edit([ARFetch("id", false, true)] ActivityEnrolment enrolment)
        {
            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("activities", Activity.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] ActivityEnrolment enrolment)
        {
            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("activities", Activity.FindAll());
                RedirectToAction("edit");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] ActivityEnrolment enrolment)
        {
            enrolment.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}