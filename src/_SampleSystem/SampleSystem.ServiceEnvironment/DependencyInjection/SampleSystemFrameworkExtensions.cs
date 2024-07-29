using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.Setup;
using Framework.Events.Legacy;
using Framework.Notification;

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
                tsSettings.AddTargetSystem<ISampleSystemBLLContext, PersistentDomainObjectBase>(true, true));

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
        services.AddScoped<IEmployeeSource, Framework.Configuration.BLL.EmployeeSource<Employee>>();

        // For subscription
        services.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(typeof(EmployeeUpdateSubscription).Assembly));

        // For legacy audit
        services.AddKeyedSingleton("DTO", TypeResolverHelper.Create(TypeSource.FromSample<BusinessUnitSimpleDTO>(), TypeSearchMode.Both));

        return services;
    }
}
