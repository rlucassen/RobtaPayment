using System;

namespace RobtaPayment.Model.Entities
{
    using Castle.ActiveRecord;
    using Castle.Components.Validator;


    public abstract class Enrolment<T> : ModelBase<T>, IEnrolment where T : Enrolment<T>
    {
        private string name = string.Empty;
        private string email = string.Empty;
        private string studentNumber = string.Empty;
        private DateTime enrolmentDate = DateTime.Now;
        private bool active = true;
        
        [Property, ValidateNonEmpty]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Property, ValidateNonEmpty]
        public virtual string Email
        {
            get { return email; }
            set { email = value; }
        }

        [Property, ValidateNonEmpty]
        public virtual string StudentNumber
        {
            get { return studentNumber; }
            set { studentNumber = value; }
        }

        [Property]
        public virtual DateTime EnrolmentDate
        {
            get { return enrolmentDate; }
            set { enrolmentDate = value; }
        }

        [Property("[Active]")]
        public virtual bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public abstract bool CanBeMadeActive();
        public abstract string Url();

        [BelongsTo("[Transaction]", Cascade = CascadeEnum.Delete)]
        public virtual Transaction Transaction { get; set; }

        public virtual bool Paid
        {
            get
            {
                return Transaction != null && Transaction.Payed;
            }
        }
    }
}
