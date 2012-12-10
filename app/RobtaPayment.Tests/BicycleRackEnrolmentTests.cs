using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Tests
{
    using System.Reflection;
    using Model.Entities;
    using Model.Enums;
    using NUnit.Framework;

    [TestFixture]
    public class BicycleRackEnrolmentTests : TestBase
    {
        private BicycleRack rack;
        private BicycleRackEnrolment enrolment;

        [SetUp]
        public void Setup()
        {
            rack = new BicycleRack();
            enrolment = new BicycleRackEnrolment()
                            {Name = "Jan-Jaap", Email = "jan@jaap.jj", StudentNumber = "012345", BicycleRack = rack};
        }

        [Test]
        public void Status()
        {
            Assert.AreEqual(BicycleRackEnrolmentStatus.NewEnrolment, enrolment.Status);
        }

        [Test]
        public void IssueKey()
        {
            enrolment.IssueKey();
            Assert.AreEqual(BicycleRackEnrolmentStatus.KeyIssued, enrolment.Status);
        }

        [Test]
        public void CloseEnrolment()
        {
            enrolment.CloseEnrolment();
            Assert.AreEqual(BicycleRackEnrolmentStatus.EnrolmentFinished, enrolment.Status);
        }

    }
}
