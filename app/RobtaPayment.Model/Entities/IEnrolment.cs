namespace RobtaPayment.Model.Entities
{
    using System;
    using Castle.ActiveRecord;

    public interface IEnrolment
    {
        string Name { get; }
        string Email { get; }
        string StudentNumber { get; }
        DateTime EnrolmentDate { get; }
        Transaction Transaction { get; }
        bool Paid { get; }
        bool Active { get; set; }

        bool CanBeMadeActive();
        string Url();
    }
}