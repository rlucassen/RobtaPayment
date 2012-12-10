namespace RobtaPayment.Model.Entities
{
    using System;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    [Import(typeof(IEnrolment), "IEnrolment")]
    public class ActivityEnrolment : Enrolment<ActivityEnrolment>
    {
        
        private Activity activity;

        public ActivityEnrolment()
        {
            
        }

        public ActivityEnrolment(Activity activity)
        {
            this.activity = activity;
        }


        [BelongsTo]
        public virtual Activity Activity
        {
            get { return activity; }
            set { activity = value; }
        }

        public override bool CanBeMadeActive()
        {
            return activity.IsOpen;
        }

        public override string Url()
        {
            return String.Format("/Admin/ActivityEnrolments/edit?id={0}", Id);
        }
    }
}