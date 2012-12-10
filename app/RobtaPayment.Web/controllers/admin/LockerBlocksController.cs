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
    public class LockerBlocksController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("lockerblocks", LockerBlock.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("lockerblock", new LockerBlock());
            PropertyBag.Add("buildings", Building.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("lockerblock", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] LockerBlock lockerBlock,
                           [ARFetch("lockerblock.Building")] Building building)
        {
            lockerBlock.Building = building;
            if (!lockerBlock.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("lockerblock", lockerBlock);
                RenderView("new");
                return;
            }

            lockerBlock.SaveAndFlush();
            RedirectToAction("index");
        }

        public void Edit([ARFetch("id", false, true)] LockerBlock lockerBlock)
        {
            PropertyBag.Add("lockerblock", lockerBlock);
            PropertyBag.Add("buildings", Building.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("lockerblock", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] LockerBlock lockerBlock,
                           [ARFetch("lockerblock.Building")] Building building)
        {
            lockerBlock.Building = building;
            if (!lockerBlock.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("lockerblock", lockerBlock);
                RedirectToAction("edit");
                return;
            }

            lockerBlock.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] LockerBlock lockerBlock)
        {
            lockerBlock.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}