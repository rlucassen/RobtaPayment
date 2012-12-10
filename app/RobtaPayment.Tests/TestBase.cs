namespace RobtaPayment.Tests
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using Castle.ActiveRecord;
    using Castle.ActiveRecord.Framework.Config;
    using Model.Entities;
    using NUnit.Framework;

    public class TestBase
    {
        public SchoolYear schoolYear;
        public SchoolYear nextSchoolYear;

        protected SessionScope SessionScope;

        public TestBase()
        {
            if (!ActiveRecordStarter.IsInitialized) Initialize();
        }

        [SetUp]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            SessionScope = new SessionScope(FlushAction.Never);
            ActiveRecordStarter.CreateSchema();

            schoolYear = new SchoolYear(){Name ="2011/2012", SchoolYearStart = new DateTime(2011, 06, 01), SchoolYearEnd = new DateTime(2012, 05, 31)};
            schoolYear.SaveAndFlush();
            nextSchoolYear = new SchoolYear(){Name ="2012/2013", SchoolYearStart = new DateTime(2012, 06, 01), SchoolYearEnd = new DateTime(2013, 05, 31)};
            nextSchoolYear.SaveAndFlush();
        }

        [TearDown]
        public void TearDown()
        {
            SessionScope.Dispose();
            ActiveRecordStarter.DropSchema();
            ActiveRecordStarter.ResetInitializationFlag();
        }

        private static void Initialize()
        {
            var config = ActiveRecordSectionHandler.Instance;
            var assembly = Assembly.Load("RobtaPayment.Model");
            ActiveRecordStarter.Initialize(new[] {assembly}, config);
        }
    }
}