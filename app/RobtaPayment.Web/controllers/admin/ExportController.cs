namespace RobtaPayment.Web.controllers.admin
{
    #region

    using System;
    using System.Linq;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Entities;
    using RobtaPayment.Model.Helpers;
    using NHibernate.Criterion;

    #endregion

    [ControllerDetails(Area = "Admin")]
    public class ExportController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("schoolyears", SchoolYear.FindAll().OrderBy(s => s.SchoolYearStart));
        }

        public void EnrolmentExport([ARFetch("schoolyear")] SchoolYear schoolYear)
        {
            var crit = DetachedCriteria.For<LockerEnrolment>();
            crit.Add(Restrictions.Eq("SchoolYear", schoolYear));
            var lockerEnrolments = LockerEnrolment.FindAll(crit).ToList();


            var csvString = CsvHelper.GenerateCsvForLockerEnrolments(lockerEnrolments);
            var filename = string.Format("inschrijvingen-kluisjes-{0}.csv", DateTime.Now.ToString("yyyyMMdd-hhmm"));

            CancelView();
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Write(csvString);
        }
    }
}