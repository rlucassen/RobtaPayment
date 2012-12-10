using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobtaPayment.Web.controllers
{
    using System.Collections;
    using System.Configuration;
    using Castle.ActiveRecord;
    using Castle.MonoRail.ActiveRecordSupport;
    using Model.Entities;
    using Model.Helpers;
    using NHibernate.Criterion;

    public class LockerController : ControllerBase
    {
        protected readonly string mollieClientNumber;
        protected readonly bool mollieTestMode;

        public LockerController()
        {
            mollieClientNumber = ConfigurationManager.AppSettings["MollieClientNumber"];
            mollieTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["MollieTestMode"]);
        }

        public void Index()
        {
            PropertyBag.Add("buildings", Building.FindAll() );
        }

        public void LockerBlock([ARFetch("id", false, true)] LockerBlock lockerBlock)
        {
            PropertyBag.Add("lockerblock", lockerBlock);
            PropertyBag.Add("schoolyear", SchoolYear.CurrentSchoolYear);
        }

        public void Enrol([ARFetch("locker", false, true)] Locker locker)
        {

            PropertyBag.Add("locker", locker);
            PropertyBag.Add("schoolyear", SchoolYear.CurrentSchoolYear);
            AddAvailableSchoolyearsToPropertyBag();
            if (locker.IsFree())
            {
                PropertyBag.Add("enrolment", new LockerEnrolment(locker));
            }
        }

        public void Payment([ARDataBind("enrolment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id, Guid")] LockerEnrolment enrolment, [ARFetch("locker", false, true)] Locker locker, [ARFetch("schoolyear", false, true)] SchoolYear schoolYear)
        {
            enrolment.Locker = locker;
            enrolment.SchoolYear = schoolYear;
            if (!enrolment.Locker.IsFree(enrolment.SchoolYear))
            {
                Flash["error"] = String.Format("Dit kluisje is al verhuurd voor schooljaar {0}, kies een ander kluisje.", enrolment.SchoolYear.Name);
                RedirectToReferrer();
                return;
            }
            if (!enrolment.IsValid())
            {
                Flash["error"] = "Je hebt niet alle velden ingevuld, vul alle velden in en probeer het opnieuw.";
                PropertyBag.Add("enrolment", enrolment);
                PropertyBag.Add("locker", enrolment.Locker);
                PropertyBag.Add("schoolyear", SchoolYear.CurrentSchoolYear);
                AddAvailableSchoolyearsToPropertyBag();
                RenderView("enrol");
                return;
            }
            var crit = DetachedCriteria.For<LockerEnrolment>();
            crit.Add(Restrictions.Eq("StudentNumber", enrolment.StudentNumber));
            crit.Add(Restrictions.Eq("SchoolYear", enrolment.SchoolYear));
            if (LockerEnrolment.FindAll(crit).Any())
            {
                Flash["error"] = "Je hebt al een ander kluisje gehuurd dit schooljaar, je kunt maar 1 kluisje huren.";
                RedirectToAction("index");
                return;
            }

            var amount = Convert.ToDecimal(ConfigurationManager.AppSettings["LockerHirePrice"]) + Convert.ToDecimal(ConfigurationManager.AppSettings["LockerGuaranty"]);

            Transaction transaction = new Transaction();
            transaction.Amount = amount;
            transaction.Save();
            enrolment.Transaction = transaction;
            enrolment.SaveAndFlush();

            PropertyBag.Add("locker", locker);
            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("banks", MollieIdealHelper.GetIdealBanks(mollieClientNumber, mollieTestMode));
            PropertyBag.Add("transaction", enrolment.Transaction);
        }

        public void Renewal(string building, int lockernr)
        {
            var crit = DetachedCriteria.For<Locker>();
            crit.Add(Restrictions.Eq("Number", lockernr));
            crit.CreateAlias("LockerBlock", "lockerBlock");
            crit.CreateAlias("lockerBlock.Building", "building");
            crit.Add(Restrictions.Eq("building.Slug", building));

            Locker locker = Locker.FindFirst(crit);

            PropertyBag.Add("locker", locker);
            var nextSchoolYear = SchoolYear.CurrentSchoolYear.NextSchoolYear;
            LockerEnrolment enrolment;
            if (locker.IsFree(nextSchoolYear))
            {
                enrolment = new LockerEnrolment(locker, nextSchoolYear);
                enrolment.Name = locker.LastEnrolment.Name;
                enrolment.StudentNumber = locker.LastEnrolment.StudentNumber;
                enrolment.Email = locker.LastEnrolment.Email;
                var transaction = new Transaction();
                transaction.Amount = Convert.ToDecimal(ConfigurationManager.AppSettings["LockerHirePrice"]);
                transaction.Save();
                enrolment.Transaction = transaction;
                enrolment.SaveAndFlush();
            }
            else if (locker.LastEnrolment.SchoolYear == nextSchoolYear && locker.LastEnrolment.Transaction.Payed == false) // is er al een onbetaalde transaction?
            {
                enrolment = locker.LastEnrolment;
            }
            else
            {
                Flash["error"] = "Dit kluisje is al verhuurd.";
                return;
            }

            PropertyBag.Add("enrolment", enrolment);
            PropertyBag.Add("banks", MollieIdealHelper.GetIdealBanks(mollieClientNumber, mollieTestMode));
            PropertyBag.Add("transaction", enrolment.Transaction);
        }

        private void AddAvailableSchoolyearsToPropertyBag()
        {
            PropertyBag.Add("schoolyears", SchoolYear.FindAll().OrderBy(s => s.SchoolYearStart).Where(s => s.SchoolYearEnd > DateTime.Today));
        }
    }
}