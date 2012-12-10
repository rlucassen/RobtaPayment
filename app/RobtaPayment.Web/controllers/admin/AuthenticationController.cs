using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobtaPayment.Web.controllers.admin
{
    using Castle.MonoRail.Framework;
    using RobtaPayment.Model.Entities;
    using RobtaPayment.Model.Enums;
    using helpers;

    [ControllerDetails(Area = "Admin")]
    public class AuthenticationController : ControllerBase
    {
        public void Index(string returnUrl)
        {
            PropertyBag.Add("returnurl", returnUrl);
        }

        public void Login(string username, string password, string returnUrl)
        {
            var user = User.ValidateUser(username, password);
            if (user == null)
            {
                Flash["error"] = "Loginnaam of wachtwoord niet juist";
                RenderView("index", false);
                return;
            }

            switch (user.AccountType)
            {
                case AccountType.ReadOnly:
                case AccountType.Admin:
                    break;
                default:
                    {
                        Flash["error"] = "Deze account is niet actief";
                        RenderView("index", false);
                        return;
                    }
            }

            AuthenticationHelper.SetAuthCookie(Context, user);

            if (string.IsNullOrEmpty(returnUrl))
            {
                RedirectToUrl("/Admin/Activities/Index.rails");
            } else
            {
                RedirectToUrl(returnUrl);
            }
        }

        public void Logout(string returnUrl)
        {
            AuthenticationHelper.Logout(Context);
            RedirectToUrl(string.Format("/Admin/Authentication/Index.rails?returnUrl={0}", returnUrl));
        }
    }
}