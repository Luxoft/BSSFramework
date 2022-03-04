using System;
using System.Collections.Generic;

using Automation;
using Automation.Enums;
using Automation.Utils;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.Notification.DTO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData.Helpers;
using WorkflowSampleSystem.WebApiCore.Controllers;

using DataHelper = WorkflowSampleSystem.IntegrationTests.__Support.TestData.Helpers.DataHelper;

namespace WorkflowSampleSystem.IntegrationTests.__Support.TestData
{
    [TestClass]
    public class TestBase : IControllerEvaluatorContainer
    {
        private static readonly Lazy<TestServiceEnvironment> EnvironmentLazy = new Lazy<TestServiceEnvironment>(() => TestServiceEnvironment.IntegrationEnvironment, true);

        private DataHelper dataHelper;

        protected TestBase()
        {
            this.SetCurrentDateTime(DateTime.Now);
            this.DataHelper = new DataHelper();
            this.AuthHelper = new AuthHelper();

            // Workaround for System.Drawing.Common problem https://chowdera.com/2021/12/202112240234238356.html
            System.AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
        }

        protected TestServiceEnvironment Environment => EnvironmentLazy.Value;

        public MainWebApi MainWebApi => new(this.Environment.ServiceProvider);

        protected DataHelper DataHelper
        {
            get
            {
                this.dataHelper.AuthHelper = this.AuthHelper;
                return this.dataHelper;
            }

            set
            {
                this.dataHelper = value;
            }
        }

        protected AuthHelper AuthHelper { get; }

        protected IDateTimeService DateTimeService => this.Environment.ServiceProvider.GetRequiredService<IDateTimeService>();

        IServiceProvider IControllerEvaluatorContainer.RootServiceProvider => this.Environment.RootServiceProvider;

        protected string DatabaseName { get; } = "WorkflowSampleSystem";

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

        protected TResult EvaluateWrite<TResult>(Func<IWorkflowSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, this.GetCurrentUserName(), func);
        }

        protected TResult EvaluateRead<TResult>(Func<IWorkflowSampleSystemBLLContext, TResult> action)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.GetCurrentUserName(), ctx => action(ctx));
        }

        protected void EvaluateWrite(Action<IWorkflowSampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(
                                                            DBSessionMode.Write,
                                                            this.GetCurrentUserName(),
                                                            context =>
                                                            {
                                                                action.Invoke(context);
                                                                return Ignore.Value;
                                                            });
        }

        protected ControllerEvaluator<AuthSLJsonController> GetAuthControllerEvaluator()
        {
            return this.MainWebApi.GetControllerEvaluator<AuthSLJsonController>();
        }

        protected ControllerEvaluator<WorkflowSLJsonController> GetWorkflowControllerEvaluator()
        {
            return this.MainWebApi.GetControllerEvaluator<WorkflowSLJsonController>();
        }
    }
}
