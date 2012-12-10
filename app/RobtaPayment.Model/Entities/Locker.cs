namespace RobtaPayment.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using NHibernate.Criterion;

    [ActiveRecord]
    public class Locker : ModelBase<Locker>
    {
        private int number = 0;
        private bool keyInHouse = true;

        private LockerBlock lockerBlock;
        private IList<LockerEnrolment> enrolments = new List<LockerEnrolment>();

        [Property, ValidateNotSameValue(0)]
        public virtual int Number
        {
            get { return number; }
            set { number = value; }
        }

        [Property]
        public virtual bool KeyInHouse
        {
            get { return keyInHouse; }
            set { keyInHouse = value; }
        }

        [BelongsTo]
        public virtual LockerBlock LockerBlock
        {
            get { return lockerBlock; }
            set { lockerBlock = value; }
        }

        [HasMany(Lazy = true)]
        public virtual IList<LockerEnrolment> Enrolments
        {
            get { return enrolments; }
            set { enrolments = value; }
        }

        public virtual string Name
        {
            get { return String.Format("{0} ({1})", Number, LockerBlock.Building.Name); }
        }

        public virtual bool IsFree()
        {
            var schoolYear = SchoolYear.CurrentSchoolYear;
            return IsFree(schoolYear);
        }
        
        public virtual bool IsFree(SchoolYear schoolYear)
        {
            var crit = DetachedCriteria.For<LockerEnrolment>();
            crit.Add(Restrictions.Eq("Locker", this));
            crit.Add(Restrictions.Eq("SchoolYear", schoolYear));
            crit.Add(Restrictions.Eq("Active", true));

            return LockerEnrolment.Count(crit) == 0;
        }

        public virtual LockerEnrolment LastEnrolment
        {
            get { return enrolments.Where(e => e.Active).OrderByDescending(e => e.SchoolYear.SchoolYearStart).First(); }
        }
    }
}