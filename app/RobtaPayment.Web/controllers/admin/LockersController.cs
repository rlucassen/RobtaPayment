namespace RobtaPayment.Web.controllers.admin
{
    #region

    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Entities;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Web.filters;
    using RobtaPayment.Web.helpers;

    #endregion

    [ControllerDetails(Area = "Admin")]
    public class LockersController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("lockers", Locker.FindAll());
        }

        public void View([ARFetch("id", false, true)] Locker locker)
        {
            PropertyBag.Add("locker", locker);
            PropertyBag.Add("schoolyears", SchoolYear.FindAll().OrderBy(s => s.SchoolYearStart));
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void KeyIn([ARFetch("id", false, true)] Locker locker)
        {
            locker.KeyInHouse = true;
            locker.SaveAndFlush();
            var options = new NameValueCollection();
            options.Add("id", locker.Id.ToString());
            RedirectToAction("view", options);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void KeyOut([ARFetch("id", false, true)] Locker locker)
        {
            locker.KeyInHouse = false;
            locker.SaveAndFlush();
            var options = new NameValueCollection();
            options.Add("id", locker.Id.ToString());
            RedirectToAction("view", options);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Deactivate([ARFetch("id", false, true)] LockerEnrolment enrolment)
        {
            enrolment.Active = false;
            enrolment.SaveAndFlush();
            var options = new NameValueCollection();
            options.Add("id", enrolment.Locker.Id.ToString());
            RedirectToAction("view", options);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Activate([ARFetch("id", false, true)] LockerEnrolment enrolment)
        {
            enrolment.Active = true;
            enrolment.SaveAndFlush();
            var options = new NameValueCollection();
            options.Add("id", enrolment.Locker.Id.ToString());
            RedirectToAction("view", options);
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Enrol([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id, Guid")] LockerEnrolment enrolment,
                          [ARFetch("locker", false, true)] Locker locker, [ARFetch("schoolyear", false, true)] SchoolYear schoolYear, bool paid)
        {
            enrolment.Locker = locker;
            enrolment.SchoolYear = schoolYear;

            var transaction = new Transaction();
            transaction.Payed = paid;
            transaction.Amount = Convert.ToDecimal(ConfigurationManager.AppSettings["LockerHirePrice"]) + Convert.ToDecimal(ConfigurationManager.AppSettings["LockerGuaranty"]);
            transaction.SaveAndFlush();
            enrolment.Transaction = transaction;

            enrolment.SaveAndFlush();

            var options = new NameValueCollection();
            options.Add("id", locker.Id.ToString());
            RedirectToAction("view", options);
        }

        public void Delete([ARFetch("id", false, true)] LockerEnrolment enrolment)
        {
            enrolment.DeleteAndFlush();
            RedirectToReferrer();
        }

    }
}