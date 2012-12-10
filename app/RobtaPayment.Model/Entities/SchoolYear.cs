namespace RobtaPayment.Model.Entities
{
    using System;
    using System.Linq;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using NHibernate.Criterion;

    [ActiveRecord]
    public class SchoolYear : ModelBase<SchoolYear>
    {
        private string name;
        private DateTime schoolYearStart = DateTime.Today;
        private DateTime schoolYearEnd = DateTime.Today.AddYears(1);

        [Property, ValidateNonEmpty]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The date that you can start enrolling lockers
        /// </summary>
        [Property]
        public virtual DateTime SchoolYearStart
        {
            get { return schoolYearStart; }
            set { schoolYearStart = value; }
        }

        /// <summary>
        /// The date that all locker enrollments expire
        /// </summary>
        [Property]
        public virtual DateTime SchoolYearEnd
        {
            get { return schoolYearEnd; }
            set { schoolYearEnd = value; }
        }

        public virtual SchoolYear NextSchoolYear
        {
            get
            {
                var schoolYears = FindAll().OrderBy(s => s.SchoolYearStart).ToList();
                return schoolYears[schoolYears.IndexOf(this) + 1];
            }
        }

        public static SchoolYear CurrentSchoolYear
        {
            get
            {
                var now = DateTime.Now;
                return SchoolYearForDate(now);
            }
        }

        public static SchoolYear SchoolYearForDate(DateTime date)
        {
            var crit = DetachedCriteria.For<SchoolYear>();
            crit.Add(Restrictions.Gt("SchoolYearEnd", date));
            crit.Add(Restrictions.Le("SchoolYearStart", date));

            var years = FindAll(crit);
            if(years.Any())
                return years[0];
            return null;
        }

        public virtual string FriendlyText
        {
            get { 
                var currentSchoolYearText = string.Empty;
                if (this == SchoolYear.CurrentSchoolYear)
                    currentSchoolYearText = "- Huidig schooljaar";
                return string.Format("{0} {1}", Name, currentSchoolYearText);
            }
        }

        public virtual string FriendlyPeriod
        {
            get
            {
                var from = SchoolYearStart.ToString("dd-MM-yyyy");
                var till = SchoolYearEnd.ToString("dd-MM-yyyy");
                if (SchoolYearStart < DateTime.Today)
                    from = "VANDAAG";
                return string.Format("van {0} tot {1}", from, till);
            }
        }
    }
}