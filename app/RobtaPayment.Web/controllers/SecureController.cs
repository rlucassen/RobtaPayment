using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobtaPayment.Web.controllers
{
    using System.Reflection;
    using System.Security;
    using Castle.MonoRail.Framework;
    using RobtaPayment.Web.helpers;
    using Model.Entities;
    using filters;

    [Filter(ExecuteWhen.BeforeAction, typeof(AuthenticationFilter), ExecutionOrder = 1)]
    public class SecureController : ControllerBase
    {
        protected virtual User CurrentUser
        {
            get
            {
                return Context.CurrentUser as User;
            }
        }

        protected override object InvokeMethod(MethodInfo method, IRequest request, IDictionary<string, object> extraArgs)
        {
            try
            {
                RobtaPaymentAuthorizationAttribute[] RobtaPaymentAuthorizationAttributes = (RobtaPaymentAuthorizationAttribute[])method.GetCustomAttributes(typeof(RobtaPaymentAuthorizationAttribute), false);
                if (RobtaPaymentAuthorizationAttributes.Length > 0)
                {
                    RobtaPaymentAuthorizationAttribute RobtaPaymentAuthorizationAttribute = RobtaPaymentAuthorizationAttributes[0];
                    if (!RobtaPaymentAuthorizationAttribute.ValidAccountTypes.Contains(CurrentUser.AccountType))
                    {
                        throw new SecurityException();
                    }
                }

                return SecureInvokeMethod(method, request, extraArgs);
            }
            catch(SecurityException ex)
            {
                Flash["error"] = "Dit account heeft geen rechten voor deze functie.";
            }
            //Redirect("Admin", "Authentication", "Index");
            RedirectToReferrer();
            return null;
        }

        protected virtual object SecureInvokeMethod(MethodInfo method, IRequest request,
                                                    IDictionary<string, object> extraArgs)
        {
            if(CurrentUser == null || !CurrentUser.IsAuthenticated)
            {
                throw new SecurityException("Je bent niet ingelogd en hebt dus geen rechten");
            }
            return base.InvokeMethod(method, request, extraArgs);
        }
    }
}