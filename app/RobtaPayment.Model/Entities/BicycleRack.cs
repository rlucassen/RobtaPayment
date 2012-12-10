namespace RobtaPayment.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using Enums;

    [ActiveRecord]
    public class BicycleRack : ModelBase<BicycleRack>
    {
        private string name;
        private int places;

        private IList<BicycleRackEnrolment> enrolments = new List<BicycleRackEnrolment>();

        [HasMany(Lazy = true)]
        public virtual IList<BicycleRackEnrolment> Enrolments
        {
            get { return enrolments; }
            set { enrolments = value; }
        }

        [Property, ValidateNonEmpty]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Property]
        public virtual int Places
        {
            get { return places; }
            set { places = value; }
        }

        public virtual bool IsOpen
        {
            get { return enrolments.Count(e => e.Status != BicycleRackEnrolmentStatus.EnrolmentFinished || e.Active) < places; }
        }

        public virtual int FreePlaces
        {
            get { return places - enrolments.Count(e => e.Status != BicycleRackEnrolmentStatus.EnrolmentFinished || e.Active); }
        }
    }
}