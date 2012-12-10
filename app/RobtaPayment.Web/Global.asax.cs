namespace RobtaPayment.Web
{
    #region

    using System;
    using System.Web;
    using Castle.ActiveRecord;
    using Castle.MonoRail.Framework;
    using Castle.MonoRail.Framework.Configuration;
    using Castle.MonoRail.Framework.Container;
    using Castle.MonoRail.Framework.Helpers.ValidationStrategy;
    using Castle.MonoRail.Framework.JSGeneration;
    using Castle.MonoRail.Framework.JSGeneration.jQuery;
    using Castle.MonoRail.Framework.Routing;
    using Castle.MonoRail.Framework.Services.AjaxProxyGenerator;
    using Castle.Windsor;


    #endregion

    public class Global : HttpApplication, IContainerAccessor, IMonoRailContainerEvents, IMonoRailConfigurationEvents
    {
        private static Castle.Windsor.IWindsorContainer container;

        protected void Application_Start(object sender, EventArgs e)
        {
            container = new RobtaPaymentContainer();

            //Hiermee kun je een nieuw sql script voor de database laten genereren
            //     var filePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "createschema.sql");
            //ActiveRecordStarter.GenerateCreationScripts(filePath);

            RoutingModuleEx.Engine.Add(new PatternRoute("/")
                               .DefaultForController().Is("home")
                               .DefaultForAction().Is("index"));

            RoutingModuleEx.Engine.Add(new PatternRoute("/werkweken")
                               .DefaultForController().Is("activity")
                               .DefaultForAction().Is("index"));

            RoutingModuleEx.Engine.Add(new PatternRoute("/kluisjes")
                               .DefaultForController().Is("locker")
                               .DefaultForAction().Is("index"));

            RoutingModuleEx.Engine.Add(new PatternRoute("/fietsenstalling")
                               .DefaultForController().Is("bicycle")
                               .DefaultForAction().Is("index"));

            RoutingModuleEx.Engine.Add(new PatternRoute("/toetsen")
                               .DefaultForController().Is("exam")
                               .DefaultForAction().Is("index"));

            RoutingModuleEx.Engine.Add(new PatternRoute("/backend")
                                           .DefaultForArea().Is("admin")
                                           .DefaultForController().Is("activities")
                                           .DefaultForAction().Is("index"));

            //RoutingModuleEx.Engine.Add(new PatternRoute("/verleng/<id>")
            //                    .DefaultForController().Is("locker")
            //                    .DefaultForAction().Is("renewal")
            //                    .Restrict("id").ValidInteger);

            RoutingModuleEx.Engine.Add(new PatternRoute("/verleng/<building>/<lockernr>")
                                .DefaultForController().Is("locker")
                                .DefaultForAction().Is("renewal")
                                .Restrict("lockernr").ValidInteger);

            RoutingModuleEx.Engine.Add(
                new PatternRoute("/<controller>")
                    .DefaultForAction().Is("index")
            );

            RoutingModuleEx.Engine.Add(
                new PatternRoute("/<controller>/<action>")
                    .DefaultForController().Is("home")
                    .DefaultForAction().Is("index")
            );

            RoutingModuleEx.Engine.Add(
                new PatternRoute("/<area>/<controller>/<action>")
                    .DefaultForArea().Is("admin")
                    .DefaultForController().Is("home")
                    .DefaultForAction().Is("index")
            );
        }

        protected void Application_End(object sender, EventArgs e)
        {
            container.Dispose();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items.Add("ar.sessionscope", new SessionScope(FlushAction.Never));
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                SessionScope scope = HttpContext.Current.Items["ar.sessionscope"] as SessionScope;
                if (scope != null) scope.Dispose();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Error", "EndRequest: " + ex.Message, ex);
            }
        }

        #region IContainerAccessor Members

        public IWindsorContainer Container
        {
            get
            {
                return container;
            }
        }

        #endregion

        #region IMonoRailContainerEvents Members
        ///<summary>
        ///
        ///            Gives implementors a chance to register services into MonoRail's container.
        ///            
        ///</summary>
        ///
        ///<param name="container">The container.</param>
        public void Created(IMonoRailContainer container)
        {
        }

        ///<summary>
        ///
        ///            Gives implementors a chance to get MonoRail's services and uses it somewhere else - 
        ///            for instance, registering them on an IoC container.
        ///            
        ///</summary>
        ///
        ///<param name="container"></param>
        public void Initialized(IMonoRailContainer container)
        {
            IAjaxProxyGenerator ajaxProxyGenerator = new JQueryAjaxProxyGenerator();
            container.ServiceInitializer.Initialize(ajaxProxyGenerator, container);
            container.AjaxProxyGenerator = ajaxProxyGenerator;
        }

        #endregion

        #region IMonoRailConfigurationEvents Members

        ///<summary>
        ///
        ///            Implementors can take a chance to change MonoRail's configuration.
        ///            
        ///</summary>
        ///
        ///<param name="configuration">The configuration.</param>
        public void Configure(IMonoRailConfiguration configuration)
        {
            configuration.JSGeneratorConfiguration.AddLibrary("jquery-1.4.2", typeof(JQueryGenerator)).AddExtension(
                typeof(CommonJSExtension)).ElementGenerator.AddExtension(typeof(JQueryElementGenerator)).Done.
                BrowserValidatorIs(typeof(JQueryValidator)).SetAsDefault();
        }

        #endregion

    }
}