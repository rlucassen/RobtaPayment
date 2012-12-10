namespace RobtaPayment.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    public class Exam : ModelBase<Exam>
    {
        private string name = string.Empty;
        private double price = 0d;
        private DateTime openingDate = DateTime.Now;
        private DateTime expirationDate = DateTime.Now.AddDays(14);
        private string confirmationText = string.Empty;
        private bool amn;

        private IList<ExamEnrolment> enrolments = new List<ExamEnrolment>();

        [HasMany(Lazy = true)]
        public virtual IList<ExamEnrolment> Enrolments
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
        public virtual double Price
        {
            get { return price; }
            set { price = value; }
        }

        [Property]
        public virtual DateTime OpeningDate
        {
            get { return openingDate; }
            set { openingDate = value; }
        }

        [Property(SqlType = "NTEXT")]
        public virtual string ConfirmationText
        {
            get { return confirmationText; }
            set { confirmationText = value; }
        }

        [Property]
        public virtual bool Amn
        {
            get { return amn; }
            set { amn = value; }
        }

        public virtual void SetOpeningTime(string time)
        {
            try
            {
                TimeSpan ts = new TimeSpan(Int32.Parse(time.Split(':')[0]), Int32.Parse(time.Split(':')[1]), 0);
                OpeningDate = OpeningDate.Date + ts;
            }
            catch
            { }
        }

        [Property]
        public virtual DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }

        public virtual void SetExpirationTime(string time)
        {
            try
            {
                TimeSpan ts = new TimeSpan(Int32.Parse(time.Split(':')[0]), Int32.Parse(time.Split(':')[1]), 0);
                ExpirationDate = ExpirationDate.Date + ts;
            }
            catch
            { }
        }

        public virtual bool IsOpen
        {
            get
            {
                if (DateTime.Now > ExpirationDate || DateTime.Now < OpeningDate)
                    return false;
                return true;
            }
        }
    }
}