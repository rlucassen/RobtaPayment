using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobtaPayment.Web.helpers
{
    using System.Security;
    using System.Threading;
    using System.Web.Security;
    using Castle.ActiveRecord;
    using Castle.MonoRail.Framework;
    using Model.Entities;

    public class AuthenticationHelper
    {
        public static User Authenticate(IEngineContext context)
        {
            var cookieText = context.Request.ReadCookie(FormsAuthentication.FormsCookieName);

            if (string.IsNullOrEmpty(cookieText))
            {
                return null;
            }

            if (context.CurrentUser.Identity.IsAuthenticated == false)
                throw new SecurityException("Er is geen gebruiker ingelogd");

            int userId;

            var name = context.CurrentUser.Identity.Name;
            name = name.Replace(typeof(User).FullName + "_", string.Empty);
            var result = int.TryParse(name, out userId);


            if (result == false)
            {
                throw new SecurityException("De gebruikersidentificatie is niet geldig");
            }

            var user = User.Find(userId);

            if (user == null)
            {
                throw new SecurityException("Deze gebruiker staat niet in de database");
            }

            context.CurrentUser = user;
            Thread.CurrentPrincipal = user;
            return user;
        }


        public static void SetAuthCookie(IEngineContext context, User user)
        {
            var username = user.GetType().FullName + "_" + user.Id;
            FormsAuthentication.SetAuthCookie(username, true);
            context.CurrentUser = user;
            Thread.CurrentPrincipal = user;
        }

        public static void Logout(IEngineContext context)
        {
            FormsAuthentication.SignOut();
        }
    }
}