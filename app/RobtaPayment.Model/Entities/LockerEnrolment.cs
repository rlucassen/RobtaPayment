namespace RobtaPayment.Model.Entities
{
    using System;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    [Import(typeof(IEnrolment), "IEnrolment")]
    public class LockerEnrolment : Enrolment<LockerEnrolment>
    {

        private Locker locker;
        private SchoolYear schoolYear;

        public LockerEnrolment()
        {
            
        }

        public LockerEnrolment(Locker locker)
        {
            this.schoolYear = SchoolYear.CurrentSchoolYear;
            this.locker = locker;
        }

        public LockerEnrolment(Locker locker, SchoolYear schoolYear)
        {
            this.schoolYear = schoolYear;
            this.locker = locker;
        }

        [BelongsTo]
        public virtual Locker Locker
        {
            get { return locker; }
            set { locker = value; }
        }

        [BelongsTo]
        public virtual SchoolYear SchoolYear
        {
            get { return schoolYear; }
            set { schoolYear = value; }
        }

        public override bool CanBeMadeActive()
        {
            return locker.IsFree();
        }

        public override string Url()
        {
            return String.Format("/Admin/Lockers/view?id={0}", locker.Id);
        }
    }
}