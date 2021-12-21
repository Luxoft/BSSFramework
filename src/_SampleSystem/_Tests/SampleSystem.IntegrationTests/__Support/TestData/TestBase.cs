using System;
using System.Collections.Generic;

using Automation;
using Automation.Enums;
using Automation.Utils;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.Notification;
using Framework.Notification.DTO;
using Framework.Workflow.BLL;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.__Support.Utils.Framework;
using SampleSystem.WebApiCore.Controllers.Main;
using SampleSystem.WebApiCore.Controllers;
using SampleSystem.WebApiCore.CustomReports;

using DataHelper = SampleSystem.IntegrationTests.__Support.TestData.Helpers.DataHelper;

namespace SampleSystem.IntegrationTests.__Support.TestData
{
    [TestClass]
    public class TestBase
    {
        private static readonly Lazy<TestServiceEnvironment> EnvironmentLazy = new Lazy<TestServiceEnvironment>(() => TestServiceEnvironment.IntegrationEnvironment, true);

        private DataHelper dataHelper;

        protected TestBase()
        {
            this.SetCurrentDateTime(DateTime.Now);
            this.DataHelper = new DataHelper();
            this.AuthHelper = new AuthHelper();
        }

        protected TestServiceEnvironment Environment => EnvironmentLazy.Value;

        protected DataHelper DataHelper
        {
            get
            {
                this.dataHelper.AuthHelper = this.AuthHelper;
                this.dataHelper.PrincipalName = this.AuthHelper.PrincipalName;
                return this.dataHelper;
            }

            set
            {
                this.dataHelper = value;
            }
        }

        protected AuthHelper AuthHelper { get; }

        protected string PrincipalName { get; set; }

        protected IDateTimeService DateTimeService => new IntegrationTestDateTimeService();

        protected string DatabaseName { get; } = "SampleSystem";

        protected string DefaultDatabaseServer { get; } = InitializeAndCleanup.DatabaseUtil.ConnectionSettings.DataSource;

        [TestInitialize]
        public void TestBaseInitialize()
        {
            switch (ConfigUtil.TestRunMode)
            {
                case TestRunMode.DefaultRunModeOnEmptyDatabase:
                case TestRunMode.RestoreDatabaseUsingAttach:
                    AssemblyInitializeAndCleanup.RunAction("Drop Database", CoreDatabaseUtil.Drop);
                    AssemblyInitializeAndCleanup.RunAction("Restore Databases", CoreDatabaseUtil.AttachDatabase);
                    break;
            }

            this.DataHelper.Environment = this.Environment;
            this.AuthHelper.Environment = this.Environment;
            this.AuthHelper.LoginAs();

            this.ClearNotifications();
            this.ClearIntegrationEvents();

            this.SetCurrentDateTime(DateTime.Now);
        }

        [TestCleanup]
        public void BaseTestCleanup()
        {
            this.ClearNotifications();
            this.ClearIntegrationEvents();

            this.SetCurrentDateTime(DateTime.Now);
        }

        /// <summary>
        /// Set Date for DateTimeService <c>this.Context.DateTimeService.Today</c>
        /// </summary>
        /// <param name="dateTime"></param>
        protected void SetCurrentDateTime(DateTime? dateTime)
        {
            IntegrationTestDateTimeService.CurrentDate = dateTime;
        }

        /// <summary>
        /// Set Date for DateTimeService <c>this.Context.DateTimeService.Today</c>
        /// </summary>
        /// <remarks>Day in Month is used to calculate needed DateTime</remarks>
        /// <param name="day">Set day in Month</param>
        /// <param name="month">Current Month is used by default</param>
        protected void SetCurrentDay(int day, DateTime? month = null)
        {
            var date = month ?? DateTime.Today.ToMonth().StartDate;

            IntegrationTestDateTimeService.CurrentDate = new DateTime(date.Year, date.Month, 1).SubtractDay();
        }

        /// <summary>
        /// Отчистка интеграционных евентов
        /// </summary>
        protected virtual void ClearIntegrationEvents()
        {
            this.ClearModifications();

            this.EvaluateWrite(context =>
            {
                var bll = context.Configuration.Logics.Default.Create<Framework.Configuration.Domain.DomainObjectEvent>();

                bll.Remove(bll.GetFullList());
            });
        }

        /// <summary>
        /// Получение интегационных евентов
        /// </summary>
        /// <returns></returns>
        protected List<T> GetIntegrationEvents<T>(string queueTag = "default")
        {
            var serializeType = typeof(T).FullName;

            return this.EvaluateRead(
                context => context.Configuration.Logics.DomainObjectEvent
                                  .GetObjectsBy(v => v.SerializeType == serializeType && v.QueueTag == queueTag)
                                  .ToList(obj => DataContractSerializerHelper.Deserialize<T>(obj.SerializeData)));
        }

        /// <summary>
        /// Получение списка нотификаций
        /// </summary>
        /// <returns></returns>
        protected List<NotificationEventDTO> GetNotifications()
        {
            return this.EvaluateRead(
                context => context.Configuration.Logics.DomainObjectNotification.GetFullList()
                                  .ToList(obj => DataContractSerializerHelper.Deserialize<NotificationEventDTO>(obj.SerializeData)));
        }

        /// <summary>
        /// Отчистка списка нотифицаций
        /// </summary>
        protected void ClearNotifications()
        {
            this.EvaluateWrite(context => context.Configuration.Logics.DomainObjectNotification.Pipe(bll => bll.Remove(bll.GetFullList())));
        }

        /// <summary>
        /// Получение списка модификаций
        /// </summary>
        /// <returns></returns>
        protected List<ObjectModificationInfoDTO<Guid>> GetModifications()
        {
            return this.EvaluateRead(context =>

                context.Configuration.Logics.DomainObjectModification.GetFullList()
                       .ToList(mod => new ObjectModificationInfoDTO<Guid> { Identity = mod.DomainObjectId, ModificationType = mod.Type, Revision = mod.Revision, TypeInfoDescription = new TypeInfoDescriptionDTO(mod.DomainType) }));
        }

        /// <summary>
        /// Отчистка списка модификаций
        /// </summary>
        protected void ClearModifications()
        {
            this.EvaluateWrite(context => context.Configuration.Logics.DomainObjectModification.Pipe(bll => bll.Remove(bll.GetFullList())));
        }

        /// <summary>
        /// Отчистка существующих джобов и их результатов
        /// </summary>
        protected void ClearRegularJobs()
        {
            this.EvaluateWrite(context =>
            {
                context.Logics.RegularJobResult.Pipe(bll => bll.Remove(bll.GetFullList()));

                context.Configuration.Logics.RegularJob.Pipe(bll => bll.Remove(bll.GetFullList()));
            });
        }

        /// <summary>
        /// Отчистка виртуальной MSMQ-очереди
        /// </summary>
        protected void ClearRegularJobsMessages()
        {
            FakeRegularJobMessageSender.Instance.Queue.Clear();
        }

        /// <summary>
        /// Получение сообщений из витуальной MSMQ-очереди
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<RunRegularJobModelStrictDTO> GetRegularJobsMessages()
        {
            return FakeRegularJobMessageSender.Instance.Queue;
        }

        protected TResult EvaluateWrite<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, this.PrincipalName, func);
        }

        protected void EvaluateRead(Action<ISampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, action);
        }

        protected T EvaluateRead<T>(Func<ISampleSystemBLLContext, T> action)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, action);
        }


        protected void EvaluateWrite(Action<ISampleSystemBLLContext> func)
        {
            this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Write,
                this.PrincipalName,
                context =>
                {
                    func(context);
                    return Ignore.Value;
                });
        }


        protected AuthSLJsonController GetAuthorizationController()
        {
            return this.GetController<AuthSLJsonController> ();
        }

        protected ConfigSLJsonController GetConfigurationController()
        {
            return this.GetController<ConfigSLJsonController>();
        }

        protected WorkflowSLJsonController GetWorkflowController()
        {
            return this.GetController<WorkflowSLJsonController>();
        }


        protected TController GetController<TController>(string principalName = null)
            where TController : ControllerBase, IApiControllerBase
        {
            return this.DataHelper.GetController<TController>(principalName);
        }
    }
}
