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
    public class EnrolmentTests : TestBase
    {
        private Activity activity;
        private ActivityEnrolment enrolment;
        private Transaction transaction;

        [SetUp]
        public void Setup()
        {
            activity = new Activity() { Name = "test", MaximumEnrolments = 10};
            enrolment = new ActivityEnrolment()
                            {Name = "Jan-Jaap", Email = "jan@jaap.jj", StudentNumber = "012345", Activity = activity};
            transaction = new Transaction();
            enrolment.Transaction = transaction;
        }

        [Test]
        public void ShouldNotBeValidWithoutAName()
        {
            enrolment.Name = "";

            bool validation = enrolment.IsValid();

            Assert.IsTrue(enrolment.PropertiesValidationErrorMessages.Contains(typeof(ActivityEnrolment).GetProperty("Name")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAName()
        {
            enrolment.Name = "test";

            bool validation = enrolment.IsValid();

            Assert.IsFalse(enrolment.PropertiesValidationErrorMessages.Contains(typeof(ActivityEnrolment).GetProperty("Name")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldNotBeValidWithoutAnEmail()
        {
            enrolment.Email = "";

            bool validation = enrolment.IsValid();

            Assert.IsTrue(enrolment.PropertiesValidationErrorMessages.Contains(typeof(ActivityEnrolment).GetProperty("Email")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAnEmail()
        {
            enrolment.Email = "test@domain.com";

            bool validation = enrolment.IsValid();

            Assert.IsFalse(enrolment.PropertiesValidationErrorMessages.Contains(typeof(ActivityEnrolment).GetProperty("Email")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldNotBeValidWithoutAStudentNumber()
        {
            enrolment.StudentNumber = "";

            bool validation = enrolment.IsValid();

            Assert.IsTrue(enrolment.PropertiesValidationErrorMessages.Contains(typeof(ActivityEnrolment).GetProperty("StudentNumber")));
            Assert.IsFalse(validation);
        }

        [Test]
        public void ShouldBeValidWithAStudentNumber()
        {
            enrolment.StudentNumber = "123456";

            bool validation = enrolment.IsValid();

            Assert.IsFalse(enrolment.PropertiesValidationErrorMessages.Contains(typeof(ActivityEnrolment).GetProperty("StudentNumber")));
            Assert.IsTrue(validation);
        }

        [Test]
        public void ShouldReturnPaidIfTransactionIsPaid()
        {
            transaction.Payed = true;
            Assert.IsTrue(enrolment.Paid);
        }

        [Test]
        public void ShouldReturnUnpaidIfTransactionIsUnpaid()
        {
            transaction.Payed = false;
            Assert.IsFalse(enrolment.Paid);
        }

        [Test]
        public void ShouldReturnUnpaidIfTransactionIsNull()
        {
            enrolment.Transaction = null;
            Assert.IsFalse(enrolment.Paid);
        }
    }
}
