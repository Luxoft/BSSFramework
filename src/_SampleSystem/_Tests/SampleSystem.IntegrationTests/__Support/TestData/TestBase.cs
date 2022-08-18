using System;
using System.Collections.Generic;

using Automation;
using Automation.Enums;
using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.Notification.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.WebApiCore.Controllers;

using DataHelper = SampleSystem.IntegrationTests.__Support.TestData.Helpers.DataHelper;

namespace SampleSystem.IntegrationTests.__Support.TestData
{
    [TestClass]
    public class TestBase : IRootServiceProviderContainer
    {
        protected TestBase()
        {
            // Workaround for System.Drawing.Common problem https://chowdera.com/2021/12/202112240234238356.html
            System.AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
        }

        public IServiceProvider RootServiceProvider { get; private set; }

        public MainWebApi MainWebApi => new(this.RootServiceProvider);

        public MainAuditWebApi MainAuditWebApi => new(this.RootServiceProvider);

        protected DataHelper DataHelper => this.RootServiceProvider.GetService<DataHelper>();

        protected AuthHelper AuthHelper => this.RootServiceProvider.GetService<AuthHelper>();

        protected IDatabaseUtil DatabaseUtil => this.RootServiceProvider.GetService<IDatabaseUtil>();

        protected TestDateTimeService DateTimeService => this.RootServiceProvider.GetRequiredService<TestDateTimeService>();

        [TestInitialize]
        public void TestBaseInitialize()
        {
            this.RootServiceProvider = SampleSystemTestRootServiceProvider.Create();

            switch (ConfigUtil.TestRunMode)
            {
                case TestRunMode.DefaultRunModeOnEmptyDatabase:
                case TestRunMode.RestoreDatabaseUsingAttach:
                    AssemblyInitializeAndCleanup.RunAction("Drop Database", this.DatabaseUtil.DropDatabase);
                    AssemblyInitializeAndCleanup.RunAction("Restore Databases", this.DatabaseUtil.AttachDatabase);
                    break;
            }

            this.ClearNotifications();
            this.ClearIntegrationEvents();
        }

        [TestCleanup]
        public void BaseTestCleanup()
        {
            if (ConfigUtil.UseLocalDb || ConfigUtil.TestRunMode == TestRunMode.DefaultRunModeOnEmptyDatabase)
            {
                AssemblyInitializeAndCleanup.RunAction("Drop Database", this.DatabaseUtil.DropDatabase);
            }

            this.RootServiceProvider.GetRequiredService<NHibSessionEnvironment>().Dispose();
        }

        /// <summary>
        /// Set Date for DateTimeService <c>this.Context.DateTimeService.Today</c>
        /// </summary>
        /// <param name="dateTime"></param>
        protected void SetCurrentDateTime(DateTime dateTime)
        {
            this.DateTimeService.SetCurrentDateTime(dateTime);
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
                                  .GetListBy(v => v.SerializeType == serializeType && v.QueueTag == queueTag)
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

        public ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
                where TController : ControllerBase
        {
            return this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(principalName);
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
