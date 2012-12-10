using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Model.Entities
{
    using System.Security.Principal;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using RobtaPayment.Model.Enums;
    using Helpers;
    using Interfaces;
    using NHibernate.Criterion;

    [ActiveRecord("[User]", Lazy = false)]
    public class User : ModelBase<User>, IUser
    {
        public virtual bool IsInRole(string role)
        {
            return IsInRole((AccountType)Enum.Parse(typeof (AccountType), role));
        }

        public virtual bool IsInRole(AccountType accountType)
        {
            return this.AccountType == accountType;
        }

        public virtual IIdentity Identity
        {
            get { return this; }
        }

        [Property]
        public virtual string Salt { get; set; }

        [Property, ValidateNonEmpty]
        public virtual string Password { get; set; }

        [Property, ValidateNonEmpty]
        public virtual string Name { get; set; }

        [Property]
        public virtual AccountType AccountType { get; set; }

        public virtual string AuthenticationType
        {
            get { return "Forms"; }
        }

        public virtual bool IsAuthenticated
        {
            get { return true; }
        }

        public static User ValidateUser(string name, string password)
        {
            var user = FindAll(Restrictions.Eq("Name", name));
            if (user.Length != 1) return null;
            var comparePassword = PasswordHelper.ComparePassword(password, user[0]);
            return comparePassword ? user[0] : null;
        }


    }
}
