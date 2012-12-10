namespace RobtaPayment.Model.Entities
{
    using System;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    [Import(typeof(IEnrolment), "IEnrolment")]
    public class ExamEnrolment : Enrolment<ExamEnrolment>
    {
        private string preposition = string.Empty;
        private string lastname = string.Empty;
        private bool present = false;
        private string address = string.Empty;
        private string place = string.Empty;

        private Exam exam;
        private Education education;

        public ExamEnrolment()
        {
            StudentNumber = "0";
        }

        public ExamEnrolment(Exam exam)
        {
            this.exam = exam;
            StudentNumber = "0";
        }

        [BelongsTo]
        public virtual Exam Exam
        {
            get { return exam; }
            set { exam = value; }
        }

        [BelongsTo]
        public virtual Education Education
        {
            get { return education; }
            set { education = value; }
        }

        [Property]
        public virtual string Preposition
        {
            get { return preposition; }
            set { preposition = value; }
        }

        [Property, ValidateNonEmpty]
        public virtual string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        [Property]
        public virtual bool Present
        {
            get { return present; }
            set { present = value; }
        }

        [Property]
        public virtual string Address
        {
            get { return address; }
            set { address = value; }
        }

        [Property]
        public virtual string Place
        {
            get { return place; }
            set { place = value; }
        }

        public virtual string FullName
        {
            get { return string.Format("{0} {1} {2}", this.Name, this.Preposition, this.Lastname); }
        }

        public override bool CanBeMadeActive()
        {
            return true;
        }

        public override string Url()
        {
            return String.Format("/Admin/ExamEnrolments/edit?id={0}", Id);
        }

    }
}