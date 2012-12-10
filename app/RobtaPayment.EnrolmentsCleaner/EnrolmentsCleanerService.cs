using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RobtaPayment.EnrolmentsCleaner
{
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Timers;
    using Castle.ActiveRecord;
    using Castle.Core.Logging;
    using Model.Entities;
    using NHibernate.Criterion;
    using log4net;
    using log4net.Config;
    using Timer = System.Timers.Timer;

    public partial class EnrolmentsCleanerService : ServiceBase
    {
        private readonly Timer timer = new Timer(60000);
        private bool needToStop;
        private Thread workerThread;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime deactivationDate;
        private DateTime deleteDate;

        public EnrolmentsCleanerService()
        {
            InitializeComponent();

            timer.AutoReset = true;
            timer.Elapsed += TimerElapsed;
            timer.Start();
            //workerThread = new Thread(ThreadedExecute);
            AutoLog = true;
        }

        private void ThreadedExecute()
        {
            logger.Debug("Thread executing");

            deleteDate = DateTime.Now.AddMinutes(Int32.Parse(ConfigurationManager.AppSettings["TimeToDeleteEnrolmentsInMinutes"])*-1);
            deactivationDate = DateTime.Now.AddMinutes(Int32.Parse(ConfigurationManager.AppSettings["TimeToDeactivateEnrolmentsInMinutes"]) * -1);

            ProcessEnrolments<ActivityEnrolment>();
            ProcessEnrolments<LockerEnrolment>();
            ProcessEnrolments<BicycleRackEnrolment>();
            
            if (needToStop)
                return;

            logger.Debug("Thread executed");
            logger.Debug("Next thread is executed in 60 seconds, unless signalled to stop");
        }

        protected void ProcessEnrolments<T>() where T : Enrolment<T>
        {
            
            using (new SessionScope(FlushAction.Auto))
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    // Verwijderen van enrolments
                    DetachedCriteria deleteCriteria = DetachedCriteria.For<T>();
                    deleteCriteria.Add(Restrictions.Le("EnrolmentDate", deleteDate));
                    deleteCriteria.CreateAlias("Transaction", "transaction");
                    deleteCriteria.Add(Restrictions.Eq("transaction.Payed", false));
                    var deleteEnrolments = Enrolment<T>.FindAll(deleteCriteria);

                    logger.DebugFormat("{0} {1} found for deletion", deleteEnrolments.Count(), typeof(T).Name);

                    foreach (var enrolment in deleteEnrolments)
                    {
                        enrolment.Delete();
                    }

                    // Deactiveren van enrolments
                    DetachedCriteria deactivateCriteria = DetachedCriteria.For<T>();
                    deactivateCriteria.Add(Restrictions.Le("EnrolmentDate", deactivationDate));
                    deactivateCriteria.CreateAlias("Transaction", "transaction");
                    deactivateCriteria.Add(Restrictions.Eq("transaction.Payed", false));
                    var deactivateEnrolments = Enrolment<T>.FindAll(deactivateCriteria);

                    logger.DebugFormat("{0} {1} found for deactivation", deactivateEnrolments.Count(), typeof(T).Name);

                    foreach (var enrolment in deactivateEnrolments)
                    {
                        enrolment.Active = false;
                        enrolment.Save();
                    }
                    transactionScope.VoteCommit();
                }
                catch (Exception e)
                {
                    transactionScope.VoteRollBack();
                    logger.ErrorFormat("Error in thread: {0}", e.Message);
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Break();
            logger.Debug("Service Started");
        }

        protected override void OnStop()
        {
            logger.Debug("Service Stopped");
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            workerThread = new Thread(ThreadedExecute);
            workerThread.Start();
        }
    }
}
