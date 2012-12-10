namespace RobtaPayment.Web.helpers
{
    #region

    using System;
    using RobtaPayment.Model.Enums;

    #endregion

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RobtaPaymentAuthorizationAttribute : Attribute
    {
        public AccountType[] ValidAccountTypes { get; set; }

        public RobtaPaymentAuthorizationAttribute()
        {
        }

        public RobtaPaymentAuthorizationAttribute(params AccountType[] validAccountTypes)
        {
            this.ValidAccountTypes = validAccountTypes;
        }
    }
}