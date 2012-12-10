namespace RobtaPayment.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    public class Activity : ModelBase<Activity>
    {
        private string name = string.Empty;
        private double downPaymentPrice = 0d;
        private int maximumEnrolments = 0;
        private DateTime openingDate = DateTime.Now;
        private DateTime expirationDate = DateTime.Now.AddDays(7);

        private IList<ActivityEnrolment> enrolments = new List<ActivityEnrolment>();

        [HasMany(Lazy = true)]
        public virtual IList<ActivityEnrolment> Enrolments
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
        public virtual double DownPaymentPrice
        {
            get { return downPaymentPrice; }
            set { downPaymentPrice = value; }
        }

        [Property, ValidateNotSameValue(0)]
        public virtual int MaximumEnrolments
        {
            get { return maximumEnrolments; }
            set { maximumEnrolments = value; }
        }
        
        [Property]
        public virtual DateTime OpeningDate
        {
            get { return openingDate; }
            set { openingDate = value; }
        }

        public virtual void SetOpeningTime(string time)
        {
            try
            {
                TimeSpan ts = new TimeSpan(Int32.Parse(time.Split(':')[0]), Int32.Parse(time.Split(':')[1]), 0);
                OpeningDate = OpeningDate.Date + ts;
            }catch
            {}
        }

        [Property, ValidateIsGreater(IsGreaterValidationType.DateTime, "OpeningDate")]
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
            {}
        }

        public virtual bool IsOpen
        {
            get
            {
                if (DateTime.Now > ExpirationDate || DateTime.Now < OpeningDate || Enrolments.Count(e => e.Active) >= MaximumEnrolments)
                    return false;
                return true;
            }
        }

        public virtual int FreePlaces
        {
            get { return MaximumEnrolments - Enrolments.Count(e => e.Active); }
        }

        public virtual double PercentageBlocked
        {
            get { return Math.Round((Enrolments.Count(e => e.Active)*1.0/MaximumEnrolments*1.0)*100.0); }
        }

        public virtual string ListName
        {
            get { return string.Format("{0}{1}", Name, IsOpen ? "" : " (vol of verlopen)"); }
        }
    }
}