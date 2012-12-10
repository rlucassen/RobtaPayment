using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobtaPayment.Web.controllers.admin
{
    using System.Collections.Specialized;
    using System.Configuration;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Web.filters;
    using RobtaPayment.Web.helpers;
    using Model.Entities;
    using Model.Enums;
    using NHibernate.Criterion;

    [ControllerDetails(Area = "Admin")]
    public class BicycleRackEnrolmentsController : SecureController
    {
        public void Index(string query)
        {
            PropertyBag.Add("query", query);
            if (string.IsNullOrEmpty(query))
            {
                PropertyBag.Add("enrolments", BicycleRackEnrolment.FindAll());
            }
            else
            {
                var crit = DetachedCriteria.For<BicycleRackEnrolment>();
                crit.Add(Restrictions.Or(Restrictions.Like("StudentNumber", string.Format("%{0}%", query)), Restrictions.Like("Name", string.Format("%{0}%", query))));
                PropertyBag.Add("enrolments", BicycleRackEnrolment.FindAll(crit));
            }
        }

        public void New()
        {
            PropertyBag.Add("enrolment", new BicycleRackEnrolment());
            PropertyBag.Add("racks", BicycleRack.FindAll());
        }

        public void Create([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] BicycleRackEnrolment enrolment)
        {

            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevult.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("racks", BicycleRack.FindAll());
                RenderView("new");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");

        }

        public void SetPayed([ARFetch("id", false, true)] BicycleRackEnrolment enrolment)
        {
            Transaction transaction = new Transaction { Amount = Convert.ToDecimal(ConfigurationManager.AppSettings["BicycleRackPrice"]), Payed = true };
            enrolment.Transaction = transaction;
            transaction.SaveAndFlush();
            enrolment.SaveAndFlush();

            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("racks", BicycleRack.FindAll());
            RenderView("edit");
        }

        public void Edit([ARFetch("id", false, true)] BicycleRackEnrolment enrolment)
        {
            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("racks", BicycleRack.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] BicycleRackEnrolment enrolment)
        {
            if (!enrolment.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("racks", BicycleRack.FindAll());
                RedirectToAction("edit");
                return;
            }

            enrolment.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] BicycleRackEnrolment enrolment)
        {
            enrolment.DeleteAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void KeyIn([ARFetch("id", false, true)] BicycleRackEnrolment enrolment)
        {
            enrolment.CloseEnrolment();
            enrolment.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void KeyOut([ARFetch("id", false, true)] BicycleRackEnrolment enrolment)
        {
            enrolment.IssueKey();
            enrolment.SaveAndFlush();
            RedirectToAction("index");
        }

    }
}