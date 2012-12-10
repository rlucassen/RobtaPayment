namespace RobtaPayment.Model.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RobtaPayment.Model.Enums;

    public static class ContributionTypeTranslator
    {
        public static string GetString(ContributionType enumValue)
        {
            switch (enumValue)
            {
                case ContributionType.TwoYears:
                    return "2-jarige opleiding";
                case ContributionType.ThreeYears:
                    return "3-jarige opleiding";
                case ContributionType.FourYears:
                    return "4-jarige opleiding";
                default :
                    return enumValue.ToString();
            }
        }

        public static IList<KeyValuePair<int, string>> GetKeyValuePairs(params ContributionType[] contributionTypes)
        {
            if (contributionTypes.Length == 0)
                return new List<KeyValuePair<int, string>>();
            
            return contributionTypes.Select(contributionType => new KeyValuePair<int, string>((int) contributionType, GetString(contributionType))).ToList();
        }


        public static IList<KeyValuePair<int,string>> GetAllKeyValuePairs()
        {
            ContributionType[] contributionTypes = (ContributionType[]) Enum.GetValues(typeof (ContributionType));
            return GetKeyValuePairs(contributionTypes);
        }
    }
}