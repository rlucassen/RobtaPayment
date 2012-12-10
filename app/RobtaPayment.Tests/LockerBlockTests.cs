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
    public class LockerBlockTests : TestBase
    {
        private LockerBlock lockerBlock;
        private Building building;

        [SetUp]
        public void Setup()
        {
            building = new Building() { Name = "test" };
            lockerBlock = new LockerBlock() { Name = "test", Building = building};
        }

        [Test]
        public void ShouldNotBeValidWithoutAName()
        {
            lockerBlock.Name = "";

            bool validation = lockerBlock.IsValid();

            Assert.IsTrue(lockerBlock.PropertiesValidationErrorMessages.Contains(typeof(LockerBlock).GetProperty("Name")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAName()
        {
            lockerBlock.Name = "test";

            bool validation = lockerBlock.IsValid();

            Assert.IsFalse(lockerBlock.PropertiesValidationErrorMessages.Contains(typeof(LockerBlock).GetProperty("Name")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldNotBeValidWithoutABuilding()
        {
            lockerBlock.Building = null;

            bool validation = lockerBlock.IsValid();

            Assert.IsTrue(lockerBlock.PropertiesValidationErrorMessages.Contains(typeof(LockerBlock).GetProperty("Building")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithABuilding()
        {
            lockerBlock.Name = "test";

            bool validation = lockerBlock.IsValid();

            Assert.IsFalse(lockerBlock.PropertiesValidationErrorMessages.Contains(typeof(LockerBlock).GetProperty("Building")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldReturnTheTopRowOfLockers()
        {
            for(int i = 1; i <= 20; i++)
            {
                lockerBlock.Lockers.Add(new Locker(){Number = i});
            }

            var topRow = lockerBlock.LockerRow(1).Select(l => l.Number).ToList();
            Assert.Contains(1, topRow);
            Assert.Contains(6, topRow);
            Assert.Contains(11, topRow);
            Assert.Contains(16, topRow);
        }

        [Test]
        public void ShouldReturnTheBottomRowOfLockers()
        {
            for(int i = 1; i <= 20; i++)
            {
                lockerBlock.Lockers.Add(new Locker(){Number = i});
            }

            var topRow = lockerBlock.LockerRow(5).Select(l => l.Number).ToList();
            Assert.Contains(5, topRow);
            Assert.Contains(10, topRow);
            Assert.Contains(15, topRow);
            Assert.Contains(20, topRow);
        }

    }
}
