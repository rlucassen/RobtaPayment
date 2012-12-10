using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RobtaPayment.EnrolmentsCleaner
{
    using System.IO;
    using System.Reflection;
    using Castle.ActiveRecord;
    using Castle.ActiveRecord.Framework.Config;
    using Castle.Core.Logging;
    using log4net.Config;
    using log4net.Core;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "log4net.config");
            //XmlConfigurator.Configure(new FileInfo(path));
            XmlConfigurator.Configure();

            var config = ActiveRecordSectionHandler.Instance;
            var modelAssembly = Assembly.Load("RobtaPayment.Model");
            ActiveRecordStarter.Initialize(new[] { modelAssembly }, config);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new EnrolmentsCleanerService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
