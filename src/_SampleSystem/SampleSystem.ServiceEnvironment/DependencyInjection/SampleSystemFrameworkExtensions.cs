using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.Setup;
using Framework.Events.Legacy;
using Framework.Notification;
using Framework.WebApi.Utils.SL;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;
using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkExtensions
{
    public static IBssFrameworkSettings AddListeners(this IBssFrameworkSettings settings)
    {
        return settings.AddListener<SubscriptionDALListener>()
                       .AddListener<ExampleFaultDALListener>(true)
                       .AddListener<FixDomainObjectEventRevisionNumberDALListener>()
                       .AddListener<DependencyDetailEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>();
    }

    public static IBssFrameworkSettings AddSubscriptionManagers(this IBssFrameworkSettings settings)
    {
        return settings.AddSubscriptionManager<ExampleSampleSystemEventsSubscriptionManager>()
                       .AddSubscriptionManager<ExampleSampleSystemAribaEventsSubscriptionManager>();
    }

    public static IBssFrameworkSettings AddBLLSystem(this IBssFrameworkSettings settings)
    {
        return settings.AddBLLSystem<ISampleSystemBLLContext, SampleSystemBLLContext>();
    }

    public static IBssFrameworkSettings AddConfigurationTargetSystems(this IBssFrameworkSettings settings) =>
        settings.AddConfigurationTargetSystems(
            tsSettings =>
                tsSettings.AddTargetSystem<ISampleSystemBLLContext, PersistentDomainObjectBase>(
                    nameof(SampleSystem),
                    new Guid("{2D362091-7DAC-4BEC-A5AB-351B93B338D7}"),
                    true,
                    true,
                    [
                        new(typeof(Country), new Guid("{C6030B2D-16F1-4854-9FAB-8A69B7FFAC6C}")),
                        new(typeof(Employee), new Guid("{AA46DA53-9B21-4DEC-9C70-720BDA1CB198}")),
                    ]));

    public static IServiceCollection RegisterSupportLegacyServices(this IServiceCollection services)
    {
        services.AddSingleton<ISecurityRuleParser, SampleSystemSecurityRuleParser>();

        //Custom ariba sender
        services.AddScoped<SampleSystemCustomAribaLocalDBEventMessageSender>();

        //For dto mapping
        services.AddScoped<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService>();

        //For mapping domain objects to dto events
        services
            .AddScoped<IDomainEventDTOMapper<PersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<PersistentDomainObjectBase,
                ISampleSystemDTOMappingService, EventDTOBase>>();

        // For notification
        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));
        services.AddScoped<IEmployeeSource, EmployeeSource<Employee>>();

        // For subscription
        services.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(typeof(EmployeeUpdateSubscription).Assembly));

        // For legacy audit
        services.AddKeyedSingleton("DTO", TypeResolverHelper.Create(TypeSource.FromSample<BusinessUnitSimpleDTO>(), TypeSearchMode.Both));

        // For SL
        services.AddSingleton<ISlJsonCompatibilitySerializer, SlJsonCompatibilitySerializer>();

        return services;
    }
}
