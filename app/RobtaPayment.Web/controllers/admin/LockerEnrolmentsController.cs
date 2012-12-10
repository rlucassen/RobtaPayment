namespace RobtaPayment.Web.controllers.admin
{
    #region

    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Entities;
    using NHibernate.Criterion;

    #endregion

    [ControllerDetails(Area = "Admin")]
    public class LockerEnrolmentsController : SecureController
    {
         public void Index(string query, [ARFetch("schoolyear", Create = false)] SchoolYear schoolYear)
         {
             PropertyBag.Add("schoolyears", SchoolYear.FindAll());
             PropertyBag.Add("query", query);
             var crit = DetachedCriteria.For<LockerEnrolment>();
             if (!string.IsNullOrEmpty(query))
             {
                 crit.Add(Restrictions.Or(Restrictions.Like("StudentNumber", string.Format("%{0}%", query)), Restrictions.Like("Name", string.Format("%{0}%", query))));
             }
             if(schoolYear != null)
             {
                 crit.Add(Restrictions.Eq("SchoolYear", schoolYear));
             }
             PropertyBag.Add("enrolments", LockerEnrolment.FindAll(crit));
         }
    }
}