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
    public class BuidlingTests : TestBase
    {
        private Building building;

        [SetUp]
        public void Setup()
        {
            building = new Building() { Name = "test"};
        }

        [Test]
        public void ShouldNotBeValidWithoutAName()
        {
            building.Name = "";

            bool validation = building.IsValid();

            Assert.IsTrue(building.PropertiesValidationErrorMessages.Contains(typeof(Building).GetProperty("Name")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAName()
        {
            building.Name = "test";

            bool validation = building.IsValid();

            Assert.IsFalse(building.PropertiesValidationErrorMessages.Contains(typeof(Building).GetProperty("Name")));
            Assert.IsTrue(validation);
        }

    }
}
