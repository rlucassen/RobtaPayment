namespace RobtaPayment.Model.Entities
{
    using System.Collections.Generic;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Model.Helpers;
    using Helpers;

    [ActiveRecord]
    public class Contribution: ModelBase<Contribution>
    {
        [Property]
        [ValidateNonEmpty()]
        public virtual decimal Price { get; set; }

        [Property]
        [ValidateRange(RangeValidationType.Integer, ContributionType.TwoYears, ContributionType.FourYears)]
        public virtual ContributionType ContributionType { get; set; }

        [HasMany]
        public virtual IList<ContributionEnrolment> Enrolments { get; set; }

        public virtual string ListName
        {
            get { return ContributionTypeTranslator.GetString(ContributionType); }
        }
    }
}