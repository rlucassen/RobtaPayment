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
    public class LockerTests : TestBase
    {
        private Locker locker;
        private Building building;
        private LockerBlock lockerBlock;

        [SetUp]
        public void Setup()
        {
            building = new Building(){Name = "gebouw"};
            building.SaveAndFlush();
            lockerBlock = new LockerBlock(){Name = "ingang", Building = building};
            lockerBlock.SaveAndFlush();
            locker = new Locker(){LockerBlock = lockerBlock, Number = 1};
            locker.SaveAndFlush();
        }

        [Test]
        public void ShouldNotBeValidWithNumberZero()
        {
            locker.Number = 0;

            var valid = locker.IsValid();

            Assert.IsTrue(locker.PropertiesValidationErrorMessages.Contains(typeof(Locker).GetProperty("Number")));
            Assert.IsFalse(valid);
        }

        [Test]
        public void ShouldBeValidWithNumberHigherThanZero()
        {
            locker.Number = 1;

            var valid = locker.IsValid();

            Assert.IsFalse(locker.PropertiesValidationErrorMessages.Contains(typeof(Locker).GetProperty("Number")));
            Assert.IsTrue(valid);
        }

        [Test]
        public void ShouldReturnANameWhichContainsTheBuildingName()
        {
            locker.Number = 1;

            Assert.AreEqual("1 (gebouw)", locker.Name);
        }

        [Test]
        public void ShouldBeFreeIfThereIsNoEnrolment()
        {
            var schoolYear = SchoolYear.SchoolYearForDate(new DateTime(2012, 1, 1));

            Assert.IsTrue(locker.IsFree(schoolYear));
        }

        [Test]
        public void SchouldNotBeFreeIfThereIsAnEnrolment()
        {
            var enrolment = new LockerEnrolment(locker, schoolYear) { Name = "Jan-Jaap", Email = "jan@jaap.jj", StudentNumber = "012345" };
            enrolment.SaveAndFlush();
            locker.Enrolments.Add(enrolment);

            Assert.IsFalse(locker.IsFree(schoolYear));
        }

        [Test]
        public void ShouldReturnTheLastEnrolment()
        {
            var enrolment = new LockerEnrolment(locker, schoolYear) { Name = "Jan-Jaap", Email = "jan@jaap.jj", StudentNumber = "012345" };
            enrolment.SaveAndFlush();
            var enrolment2 = new LockerEnrolment(locker, nextSchoolYear) { Name = "Jan-Jaap2", Email = "jan@jaap.jj2", StudentNumber = "012346" };
            enrolment2.SaveAndFlush();
            locker.Enrolments.Add(enrolment);
            locker.Enrolments.Add(enrolment2);

            Assert.AreEqual(enrolment2, locker.LastEnrolment);
        }
    }
}
