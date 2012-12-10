using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Tests
{
    using System.Reflection;
    using Model.Entities;
    using NUnit.Framework;

    [TestFixture]
    public class SchoolYearTests : TestBase
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldNotBeValidWithoutAName()
        {
            schoolYear.Name = "";

            bool validation = schoolYear.IsValid();

            Assert.IsTrue(schoolYear.PropertiesValidationErrorMessages.Contains(typeof(SchoolYear).GetProperty("Name")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAName()
        {
            schoolYear.Name = "test";

            bool validation = schoolYear.IsValid();

            Assert.IsFalse(schoolYear.PropertiesValidationErrorMessages.Contains(typeof(SchoolYear).GetProperty("Name")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void SchouldReturnTheNextSchoolYear()
        {
            Assert.AreEqual(nextSchoolYear, schoolYear.NextSchoolYear);
        }

        [Test]
        public void SchouldReturnTheSchoolYearForADate()
        {
            var date = new DateTime(2013, 1, 1);

            Assert.AreEqual(nextSchoolYear, SchoolYear.SchoolYearForDate(date));
        }
    }
}
