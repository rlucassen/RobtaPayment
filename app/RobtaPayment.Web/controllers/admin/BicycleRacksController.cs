namespace RobtaPayment.Web.controllers.admin
{
    #region

    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Entities;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Web.filters;
    using RobtaPayment.Web.helpers;

    #endregion

    [ControllerDetails(Area = "Admin")]
    public class BicycleRacksController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("racks", BicycleRack.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("rack", new BicycleRack());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("rack", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] BicycleRack rack)
        {
            if (!rack.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("rack", rack);
                RenderView("new");
                return;
            }

            rack.SaveAndFlush();
            RedirectToAction("index");
        }

        public void Edit([ARFetch("id", false, true)] BicycleRack rack)
        {
            PropertyBag.Add("rack", rack);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("rack", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] BicycleRack rack)
        {
            if (!rack.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("rack", rack);
                RedirectToAction("edit");
                return;
            }

            rack.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] BicycleRack rack)
        {
            rack.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}