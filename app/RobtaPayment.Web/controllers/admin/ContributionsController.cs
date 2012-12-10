namespace RobtaPayment.Web.controllers.admin
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Entities;
    using RobtaPayment.Model.Enums;
    using RobtaPayment.Model.Helpers;
    using RobtaPayment.Web.filters;
    using RobtaPayment.Web.helpers;
    using NHibernate.Criterion;

    #endregion

    [ControllerDetails(Area = "Admin")]
    public class ContributionsController : SecureController
    {
        public void Index()
        {
            PropertyBag.Add("contributions", Contribution.FindAll());
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void New()
        {
            PropertyBag.Add("contribution", new Contribution());
            PropertyBag.Add("contributionTypes", ContributionTypeTranslator.GetKeyValuePairs(ContributionType.TwoYears, ContributionType.ThreeYears, ContributionType.FourYears));
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Create([ARDataBind("contribution", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey, Exclude = "Id,Guid")] Contribution contribution)
        {
            PropertyBag.Add("contributionTypes", ContributionTypeTranslator.GetKeyValuePairs(ContributionType.TwoYears, ContributionType.ThreeYears, ContributionType.FourYears));

            if (!contribution.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("contribution", contribution);
                RenderView("new");
                return;
            }

            IList<Contribution> existingContributions = Contribution.FindAll(Restrictions.Eq("ContributionType", contribution.ContributionType));
            if (existingContributions.Count > 0)
            {
                Flash["error"] = string.Format("De bijdrage voor de {0} is er al.", ContributionTypeTranslator.GetString(contribution.ContributionType));
                PropertyBag.Add("contribution", contribution);
                RenderView("new");
                return;
            }

            contribution.SaveAndFlush();
            RedirectToAction("index");
        }

        public void Edit([ARFetch("id", false, true)] Contribution contribution)
        {
            PropertyBag.Add("contribution", contribution);
            PropertyBag.Add("contributionTypes", ContributionTypeTranslator.GetKeyValuePairs(ContributionType.TwoYears, ContributionType.ThreeYears, ContributionType.FourYears));
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Update([ARDataBind("contribution", AutoLoad = AutoLoadBehavior.NullIfInvalidKey, Exclude = "Id, Guid")] Contribution contribution)
        {
            PropertyBag.Add("contributionTypes", ContributionTypeTranslator.GetKeyValuePairs(ContributionType.TwoYears, ContributionType.ThreeYears, ContributionType.FourYears));
            
            if (!contribution.IsValid())
            {
                Flash["error"] = "Niet alle velden zijn juist ingevuld.";
                PropertyBag.Add("contribution", contribution);
                RedirectToAction("edit");
                return;
            }

            IList<Contribution> existingContributions = Contribution.FindAll(Restrictions.Eq("ContributionType", contribution.ContributionType));
            if (existingContributions.Count > 0 && existingContributions.All(c => c.Id != contribution.Id))
            {
                Flash["error"] = string.Format("De bijdrage voor de {0} is er al.", ContributionTypeTranslator.GetString(contribution.ContributionType));
                PropertyBag.Add("contribution", contribution);
                RenderView("edit");
                return;
            }

            contribution.SaveAndFlush();
            RedirectToAction("index");
        }

        [RobtaPaymentAuthorization(AccountType.Admin)]
        public void Delete([ARFetch("id")] Contribution contribution)
        {
            contribution.DeleteAndFlush();
            RedirectToAction("index");
        }

        public void Csv([ARFetch("id")] Contribution contribution)
        {
            var csvString = CsvHelper.GenerateCsvForContributionsEnrolments(contribution);
            var filename = string.Format("bijdragen-{0}.csv", ContributionTypeTranslator.GetString(contribution.ContributionType));

            CancelView();
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Write(csvString);
        }
    }
}