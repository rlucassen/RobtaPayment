namespace RobtaPayment.Web.controllers.admin
{
    using System;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Web.filters;
    using RobtaPayment.Web.helpers;
    using Model.Entities;

    [ControllerDetails(Area = "Admin")]
    public class EducationsController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("educations", Education.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("education", new Education());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("education", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] Education education)
        {
            if (!education.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("education", education);
                RenderView("new");
                return;
            }

            education.SaveAndFlush();
            RedirectToAction("index");

        }

        public void Edit([ARFetch("id", false, true)] Education education)
        {
            PropertyBag.Add("education", education);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("education", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] Education education)
        {
            if (!education.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("education", education);
                RedirectToAction("edit");
                return;
            }

            education.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] Education education)
        {
            education.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}