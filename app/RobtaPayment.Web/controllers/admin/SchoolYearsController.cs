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
    public class SchoolYearsController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("schoolyears", SchoolYear.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("schoolyear", new SchoolYear());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("schoolyear", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] SchoolYear schoolYear)
        {
            if (!schoolYear.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("schoolyear", schoolYear);
                RenderView("new");
                return;
            }

            schoolYear.SaveAndFlush();
            RedirectToAction("index");
        }

        public void Edit([ARFetch("id", false, true)] SchoolYear schoolYear)
        {
            PropertyBag.Add("schoolyear", schoolYear);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("schoolyear", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] SchoolYear schoolYear)
        {
            if (!schoolYear.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("schoolyear", schoolYear);
                RedirectToAction("edit");
                return;
            }

            schoolYear.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] SchoolYear schoolYear)
        {
            schoolYear.DeleteAndFlush();
            RedirectToAction("index");
        }
    }
}