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
    public class ActivitiesController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("activities", Activity.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("activity", new Activity());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("activity", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] Activity activity, string openingTime, string expirationTime)
        {
            activity.SetOpeningTime(openingTime);
            activity.SetExpirationTime(expirationTime);

            if(!activity.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("activity", activity);
                RenderView("new");
                return;
            }

            activity.SaveAndFlush();
            RedirectToAction("index");

        }

        public void Edit([ARFetch("id", false, true)] Activity activity)
        {
            PropertyBag.Add("activity", activity);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("activity", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] Activity activity, string openingTime, string expirationTime)
        {
            activity.SetOpeningTime(openingTime);
            activity.SetExpirationTime(expirationTime);

            if (!activity.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("activity", activity);
                RedirectToAction("edit", new { Id = activity.Id});
                return;
            }

            activity.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] Activity activity)
        {
            activity.DeleteAndFlush();
            RedirectToAction("index");
        }

        public void Csv([ARFetch("id")] Activity activity)
        {
            var csvString = CsvHelper.GenerateCsvForActivityEnrolments(activity);
            var filename = string.Format("inschrijvingen-activiteit-{0}.csv", activity.Id);

            CancelView();
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Write(csvString);
        }
    }
}