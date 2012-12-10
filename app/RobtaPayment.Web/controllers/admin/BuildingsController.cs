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
    public class BuildingsController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("buildings", Building.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("building", new Building());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("building", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] Building building)
        {
            if (!building.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("building", building);
                RenderView("new");
                return;
            }

            building.SaveAndFlush();
            RedirectToAction("index");

        }

        public void Edit([ARFetch("id", false, true)] Building building)
        {
            PropertyBag.Add("building", building);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("building", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] Building building)
        {
            if (!building.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("building", building);
                RedirectToAction("edit");
                return;
            }

            building.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] Building building)
        {
            building.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}