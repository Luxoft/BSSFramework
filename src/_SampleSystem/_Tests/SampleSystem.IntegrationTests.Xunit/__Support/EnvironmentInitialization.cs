using Automation;
using Automation.Interfaces;
using Automation.ServiceEnvironment;
using Automation.Utils.DatabaseUtils;
using Automation.Xunit;
using Automation.Xunit.Interfaces;

using Bss.Platform.Events.Abstractions;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.Notification.DTO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.Support.Utils;
using SampleSystem.IntegrationTests.Xunit.__Support.Database;
using SampleSystem.IntegrationTests.Xunit.__Support.TestData;
using SampleSystem.ServiceEnvironment;
using SampleSystem.ServiceEnvironment.NHibernate;
using SampleSystem.WebApiCore.Controllers.Main;

using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass)]
[assembly: TestFramework("Automation.Xunit.AutomationCoreTestFramework", "Framework.AutomationCore.Xunit")]

namespace SampleSystem.IntegrationTests.Xunit.__Support;

public class EnvironmentInitialization : IAutomationCoreInitialization
{
    public IServiceCollection ConfigureFramework(IServiceCollection services) =>
        services
            .AddSingleton<IAssemblyInitializeAndCleanup, DiAssemblyInitializeAndCleanup>()
            .AddSingleton<ITestDatabaseGenerator, DatabaseGenerator>()
            .AddSingleton<IConfiguration>(
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .AddEnvironmentVariables($"{nameof(SampleSystem)}_")
                    .Build());

    public IServiceProvider ConfigureTestEnvironment(IServiceCollection services, IConfiguration configuration) =>
        services
            .RegisterGeneralDependencyInjection(configuration, s => s.AddExtensions(new SampleSystemNHibernateExtension()))
            .AddSingleton<SampleSystemInitializer>()
            .ReplaceScoped<IMessageSender<NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()
            .AddScoped<IIntegrationEventPublisher, TestIntegrationEventPublisher>()
            .RegisterControllers([typeof(EmployeeController).Assembly])
            .AddSingleton<DataHelper>()
            .AddSingleton<AuthHelper>()
            .AddSingleton<TestDataInitializer>()
            .AddIntegrationTestServices(options => { })
            .BuildServiceProvider();
}
