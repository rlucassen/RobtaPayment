namespace RobtaPayment.Web
{
    using System;
    using System.Reflection;
    using Castle.Core.Resource;
    using Castle.MonoRail.Framework;
    using Castle.MonoRail.WindsorExtension;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;
    using global::log4net;

    public class RobtaPaymentContainer : WindsorContainer
    {
          private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RobtaPaymentContainer()
            : base(new XmlInterpreter(new ConfigResource()))
        {
            RegisterFacilities();
            RegisterComponents();
        }

        protected void RegisterFacilities()
        {
            AddFacility("rails", new MonoRailFacility());
        }

        protected void RegisterComponents()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (assembly.FullName != null && !assembly.FullName.StartsWith("Castle.MonoRail"))
                    try
                    {
                        foreach (Type type in assembly.GetTypes())
                            if (type.IsClass && !type.IsAbstract)
                                if (typeof (IController).IsAssignableFrom(type))
                                {
                                    ControllerDetailsAttribute[] attributes =
                                        (ControllerDetailsAttribute[])
                                        type.GetCustomAttributes(typeof (ControllerDetailsAttribute), false);
                                    AddComponent(
                                        attributes != null && attributes.Length > 0 &&
                                        !string.IsNullOrEmpty(attributes[0].Name)
                                            ? attributes[0].Name.ToLowerInvariant()
                                            : type.FullName.ToLowerInvariant(), type);
                                }
                                else if (typeof (IFilter).IsAssignableFrom(type))
                                    AddComponent(type.FullName.ToLowerInvariant(), type);
                                else if (typeof (ViewComponent).IsAssignableFrom(type))
                                {
                                    ViewComponentDetailsAttribute[] attributes =
                                        (ViewComponentDetailsAttribute[])
                                        type.GetCustomAttributes(typeof (ViewComponentDetailsAttribute), false);
                                    AddComponent(
                                        attributes != null && attributes.Length > 0 &&
                                        !string.IsNullOrEmpty(attributes[0].Name)
                                            ? attributes[0].Name.ToLowerInvariant()
                                            : type.Name.ToLowerInvariant(), type);
                                }
                    }
                    catch (Exception exception)
                    {
                        Logger.Error(exception.Message, exception);
                    }
        }
    }
}