using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobtaPayment.Web.filters
{
    using Castle.MonoRail.Framework;
    using Model.Entities;
    using helpers;

    public class AuthenticationFilter : Filter
    {
        protected override bool OnBeforeAction(IEngineContext context,
                                                       IController controller,
                                                       IControllerContext controllerContext)
        {
            User user = null;
            try
            {
                user = AuthenticationHelper.Authenticate(context);
            }
            catch
            {
                SendToLoginPage(context);
                return false;
            }

            if (user == null)
            {
                SendToLoginPage(context);
                return false;
            }

            controllerContext.PropertyBag["CurrentUser"] = context.CurrentUser;
            return true;
        }

        private static void SendToLoginPage(IEngineContext context)
        {
            context.Response.RedirectToUrl(string.Format("/Admin/Authentication/Logout.rails?returnUrl={0}", context.Request.Url));
        }
    }
}