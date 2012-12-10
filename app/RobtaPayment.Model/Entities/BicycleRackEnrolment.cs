namespace RobtaPayment.Model.Entities
{
    using System;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using Enums;

    [ActiveRecord]
    [Import(typeof(IEnrolment), "IEnrolment")]
    public class BicycleRackEnrolment : Enrolment<BicycleRackEnrolment>
    {
        
        private BicycleRack bicycleRack;
        private BicycleRackEnrolmentStatus status = BicycleRackEnrolmentStatus.NewEnrolment;

        public BicycleRackEnrolment()
        {
            
        }

        public BicycleRackEnrolment(BicycleRack bicycleRack)
        {
            this.bicycleRack = bicycleRack;
        }


        [BelongsTo]
        public virtual BicycleRack BicycleRack
        {
            get { return bicycleRack; }
            set { bicycleRack = value; }
        }

        [Property]
        public virtual BicycleRackEnrolmentStatus Status
        {
            get { return status; }
            private set { status = value; }
        }

        public virtual void CloseEnrolment()
        {
            Status = BicycleRackEnrolmentStatus.EnrolmentFinished;
        }

        public virtual void IssueKey()
        {
            Status = BicycleRackEnrolmentStatus.KeyIssued;
        }

        public override bool CanBeMadeActive()
        {
            return bicycleRack.IsOpen;
        }

        public override string Url()
        {
            return String.Format("/Admin/BicycleRackEnrolments/edit?id={0}", Id);
        }
    }
}