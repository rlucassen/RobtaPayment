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
    public class ActivityTests : TestBase
    {
        private Activity activity;

        [SetUp]
        public void Setup()
        {
            activity = new Activity() { Name = "test", MaximumEnrolments = 10};
        }

        [Test]
        public void ShouldNotBeValidWithoutAName()
        {
            activity.Name = "";

            bool validation = activity.IsValid();
            
            Assert.IsTrue(activity.PropertiesValidationErrorMessages.Contains(typeof (Activity).GetProperty("Name")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAName()
        {
            activity.Name = "test";

            bool validation = activity.IsValid();

            Assert.IsFalse(activity.PropertiesValidationErrorMessages.Contains(typeof(Activity).GetProperty("Name")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldNotBeValidWithoutMaximumEnrolmentsGreaterThan0()
        {
            activity.MaximumEnrolments = 0;

            bool validation = activity.IsValid();

            Assert.IsTrue(activity.PropertiesValidationErrorMessages.Contains(typeof(Activity).GetProperty("MaximumEnrolments")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithMaximumEnrolmentsGreaterThan0()
        {
            activity.MaximumEnrolments = 10;

            bool validation = activity.IsValid();

            Assert.IsFalse(activity.PropertiesValidationErrorMessages.Contains(typeof(Activity).GetProperty("MaximumEnrolments")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldNotBeValidWithAExpirationDateBeforeTheOpeningDate()
        {
            activity.ExpirationDate = activity.OpeningDate.AddDays(-1);

            bool validation = activity.IsValid();

            Assert.IsTrue(activity.PropertiesValidationErrorMessages.Contains(typeof(Activity).GetProperty("ExpirationDate")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAExpirationDateAfterTheOpeningDate()
        {
            bool validation = activity.IsValid();

            Assert.IsFalse(activity.PropertiesValidationErrorMessages.Contains(typeof(Activity).GetProperty("MaximumEnrolments")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldReturnCorrectOpeningTimeAfterSettingWithString()
        {
            activity.OpeningDate = new DateTime(2012, 01, 01, 01, 00, 00);

            activity.SetOpeningTime("14:15");
            Assert.AreEqual(new DateTime(2012, 01, 01, 14, 15, 00), activity.OpeningDate);
        }

        [Test]
        public void ShouldReturnCorrectExpirationTimeAfterSettingWithString()
        {
            activity.OpeningDate = new DateTime(2012, 01, 01, 01, 00, 00);

            activity.SetOpeningTime("21:36");
            Assert.AreEqual(new DateTime(2012, 01, 01, 21, 36, 00), activity.OpeningDate);
        }

        [Test]
        public void ShouldBeOpenIfMaxEnrolmentsIsNotReachedAndCurrentDateIsBetweenOpeningAndExpiration()
        {
            activity.ExpirationDate = DateTime.Now.AddDays(1);
            activity.OpeningDate = DateTime.Now.AddDays(-1);
            Assert.IsTrue(activity.IsOpen);
        }

        [Test]
        public void ShouldBeClosedIfMaxEnrolmentsIsReached()
        {
            activity.MaximumEnrolments = 1;
            AddEnrolments(1);
            
            Assert.IsFalse(activity.IsOpen);
        }

        [Test]
        public void ShouldBeClosedIfExpirationDateIsInThePast()
        {
            activity.ExpirationDate = DateTime.Now.AddHours(-1);

            Assert.IsFalse(activity.IsOpen);
        }

        [Test]
        public void ShouldBeClosedIfOpeningDateIsInTheFuture()
        {
            activity.OpeningDate = DateTime.Now.AddHours(3);

            Assert.IsFalse(activity.IsOpen);
        }

        [Test]
        public void FreePlaces()
        {
            activity.MaximumEnrolments = 5;
            AddEnrolments(1);

            Assert.AreEqual(4, activity.FreePlaces);

            AddEnrolments(3);

            Assert.AreEqual(1, activity.FreePlaces);
        }

        [Test]
        public void PercentageBlocked()
        {
            activity.MaximumEnrolments = 5;
            AddEnrolments(1);

            Assert.AreEqual(20.0, activity.PercentageBlocked);

            AddEnrolments(2);

            Assert.AreEqual(60.0, activity.PercentageBlocked);
        }

        [Test]
        public void ListName()
        {
            activity.MaximumEnrolments = 2;

            Assert.AreEqual(activity.ListName, "test");

            AddEnrolments(2);

            Assert.AreEqual("test (vol of verlopen)", activity.ListName);
        }

        public void AddEnrolments(int count)
        {
            for(int i = 0; i < count; i++)
            {
                activity.Enrolments.Add(new ActivityEnrolment() { Activity = activity });
            }
        }
    }
}
