using System;
using System.Collections.Generic;

using Automation;
using Automation.Enums;
using Automation.Utils;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Notification.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers;

using DataHelper = SampleSystem.IntegrationTests.__Support.TestData.Helpers.DataHelper;

namespace SampleSystem.IntegrationTests.__Support.TestData
{
    [TestClass]
    public class TestBase : IControllerEvaluatorContainer
    {
        private static readonly Lazy<SampleSystemServiceEnvironment> EnvironmentLazy = new(() => TestServiceEnvironment.Default, true);

        private DataHelper dataHelper;

        protected TestBase()
        {
            this.SetCurrentDateTime(DateTime.Now);
            this.DataHelper = new DataHelper();
            this.AuthHelper = new AuthHelper();

            // Workaround for System.Drawing.Common problem https://chowdera.com/2021/12/202112240234238356.html
            System.AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
        }

        protected virtual SampleSystemServiceEnvironment Environment => EnvironmentLazy.Value;

        public MainWebApi MainWebApi => new(this.Environment.ServiceProvider);

        public MainAuditWebApi MainAuditWebApi => new(this.Environment.ServiceProvider);

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

        protected TResult EvaluateWrite<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, this.GetCurrentUserName(), func);
        }

        protected void EvaluateRead(Action<ISampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.GetCurrentUserName(), action);
        }

        protected T EvaluateRead<T>(Func<ISampleSystemBLLContext, T> action)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.GetCurrentUserName(), action);
        }


        protected void EvaluateWrite(Action<ISampleSystemBLLContext> func)
        {
            this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Write,
                this.GetCurrentUserName(),
                context =>
                {
                    func(context);
                    return Ignore.Value;
                });
        }

        public ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
                where TController : ControllerBase, IApiControllerBase
        {
            return this.Environment.RootServiceProvider.GetDefaultControllerEvaluator<TController>(principalName);
        }

        protected ControllerEvaluator<AuthSLJsonController> GetAuthControllerEvaluator(string principalName = null)
        {
            return this.GetControllerEvaluator<AuthSLJsonController>(principalName);
        }

        protected ControllerEvaluator<ConfigSLJsonController> GetConfigurationControllerEvaluator(string principalName = null)
        {
            return this.GetControllerEvaluator<ConfigSLJsonController>(principalName);
        }
    }
}
