using Automation.ServiceEnvironment;
using Automation.Utils.DatabaseUtils;
using Automation.Xunit;
using Automation.Xunit.ServiceEnvironment;

using Bss.Platform.Events.Abstractions;
using Bss.Testing.Xunit.Interfaces;

using CommonFramework.DependencyInjection;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Notification.DTO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.__Support.Utils;
using SampleSystem.IntegrationTests.Xunit.NHibernate.__Support.Database;
using SampleSystem.IntegrationTests.Xunit.NHibernate.__Support.TestData;
using SampleSystem.ServiceEnvironment;
using SampleSystem.ServiceEnvironment.NHibernate;
using SampleSystem.WebApiCore.Controllers.Main;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass)]
[assembly: TestFramework("Bss.Testing.Xunit.TestFramework", "Bss.Testing.Xunit")]

namespace SampleSystem.IntegrationTests.Xunit.NHibernate.__Support;

public class EnvironmentInitializer : AutomationCoreFrameworkInitializer
{
    public override IServiceCollection ConfigureFramework(IServiceCollection services) =>
        services
            .AddSingleton<IAssemblyInitializeAndCleanup, DiAssemblyInitializeAndCleanup>()
            .AddSingleton<ITestDatabaseGenerator, DatabaseGenerator>()
            .AddSingleton<IConfiguration>(
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .AddEnvironmentVariables($"{nameof(SampleSystem)}_")
                    .Build());

    public override IServiceProvider ConfigureTestEnvironment(IServiceCollection services, IConfiguration configuration) =>
        services
            .RegisterGeneralDependencyInjection(configuration, s => s.AddExtensions(new SampleSystemNHibernateExtension()))
            .AddSingleton<SampleSystemInitializer>()
            .ReplaceScoped<IMessageSender<NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()
            .AddScoped<IIntegrationEventPublisher, TestIntegrationEventPublisher>()
            .RegisterControllers([typeof(EmployeeController).Assembly])
            .AddSingleton<DataHelper>()
            .AddSingleton<TestDataInitializer>()
            .AddIntegrationTestServices()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
}
