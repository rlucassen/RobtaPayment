namespace RobtaPayment.Model.Entities
{
    using Castle.ActiveRecord;
    using RobtaPayment.Model.Enums;

    [ActiveRecord]
    [Import(typeof(IEnrolment), "IEnrolment")]
    public class ContributionEnrolment : Enrolment<ContributionEnrolment>
    {
        [BelongsTo]
        public virtual Contribution Contribution { get; set; }

        public override bool CanBeMadeActive()
        {
            return true;
        }

        public override string Url()
        {
            return string.Format("/Admin/ContributionEnrolments/edit?id={0}", Id);
        }
    }
}