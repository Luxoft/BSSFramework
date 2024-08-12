using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.Setup;
using Framework.Events.Legacy;

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

    public static IBssFrameworkSettings RegisterSupportLegacyServices(this IBssFrameworkSettings settings)
    {
        return settings.SetSecurityRuleParser<SampleSystemSecurityRuleParser>()
                       .SetSubscriptionAssembly(typeof(EmployeeUpdateSubscription).Assembly)
                       .SetNotificationDefaultMailSenderContainer<SampleSystemDefaultMailSenderContainer>()
                       .SetNotificationEmployee<Employee>()
                       .SetDTOMapping<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService, PersistentDomainObjectBase, EventDTOBase>();
    }
}
