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
    public class BicycleRackTests : TestBase
    {
        private BicycleRack rack;

        [SetUp]
        public void Setup()
        {
            rack = new BicycleRack() { Name = "test", Places = 10};
        }

        [Test]
        public void ShouldNotBeValidWithoutAName()
        {
            rack.Name = "";

            bool validation = rack.IsValid();

            Assert.IsTrue(rack.PropertiesValidationErrorMessages.Contains(typeof(BicycleRack).GetProperty("Name")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAName()
        {
            rack.Name = "test";

            bool validation = rack.IsValid();

            Assert.IsFalse(rack.PropertiesValidationErrorMessages.Contains(typeof(BicycleRack).GetProperty("Name")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldBeClosedIfPlacesIsReached()
        {
            rack.Places = 1;
            AddEnrolments(1);
            
            Assert.IsFalse(rack.IsOpen);
        }

        [Test]
        public void ShouldBeOpenIfPlacesIsNotReached()
        {
            rack.Places = 1;
            AddEnrolments(1);

            Assert.IsFalse(rack.IsOpen);
        }


        [Test]
        public void FreePlaces()
        {
            rack.Places = 5;
            AddEnrolments(1);

            Assert.AreEqual(4, rack.FreePlaces);

            AddEnrolments(3);

            Assert.AreEqual(1, rack.FreePlaces);
        }

        public void AddEnrolments(int count)
        {
            for(int i = 0; i < count; i++)
            {
                rack.Enrolments.Add(new BicycleRackEnrolment() { BicycleRack = rack });
            }
        }
    }
}
