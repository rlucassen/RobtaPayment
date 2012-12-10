using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Model.Helpers
{
    using System.Security.Cryptography;
    using Interfaces;

    public static class PasswordHelper
    {
        public static string Encrypt(string wachtwoord, string salt)
        {
            var textConverter = new UTF8Encoding();
            HashAlgorithm hash = new MD5CryptoServiceProvider();
            var tmpPassword = string.Format("{0}_{1}", wachtwoord, salt);
            var passBytes = textConverter.GetBytes(tmpPassword);
            return Convert.ToBase64String(hash.ComputeHash(passBytes));
        }

        public static bool ComparePassword(string password, IUser user)
        {
            var givenHas = Encrypt(password, user.Salt);
            return string.Compare(user.Password, givenHas) == 0;
        }

    }
}
