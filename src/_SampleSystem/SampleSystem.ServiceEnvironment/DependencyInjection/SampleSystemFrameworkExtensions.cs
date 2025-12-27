using Framework.DomainDriven._Visitors;
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
    extension(IBssFrameworkSettings settings)
    {
        public IBssFrameworkSettings AddNamedLocks()
        {
            return settings.AddNamedLocks(s => s
                                               .SetNameLockType<GenericNamedLock>(nl => nl.Name)
                                               .AddManual(typeof(BusinessUnitAncestorLink))
                                               .AddManual(typeof(ManagementUnitAncestorLink))
                                               .AddManual(typeof(LocationAncestorLink)));
        }

        public IBssFrameworkSettings AddListeners()
        {
            return settings.AddListener<SubscriptionDALListener>()
                           .AddListener<ExampleFaultDALListener>()
                           .AddListener<FixDomainObjectEventRevisionNumberDALListener>()
                           .AddListener<DependencyDetailEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>();
        }

        public IBssFrameworkSettings AddSubscriptionManagers()
        {
            return settings.AddSubscriptionManager<ExampleSampleSystemEventsSubscriptionManager>()
                           .AddSubscriptionManager<ExampleSampleSystemAribaEventsSubscriptionManager>();
        }

        public IBssFrameworkSettings AddBLLSystem()
        {
            return settings.AddBLLSystem<ISampleSystemBLLContext, SampleSystemBLLContext>();
        }

        public IBssFrameworkSettings AddConfigurationTargetSystems() =>
            settings.AddConfigurationTargetSystems(tsSettings =>
                                                       tsSettings.AddTargetSystem<ISampleSystemBLLContext, PersistentDomainObjectBase>(
                                                           nameof(SampleSystem),
                                                           new Guid("{2D362091-7DAC-4BEC-A5AB-351B93B338D7}"),
                                                           true,
                                                           true,
                                                           [
                                                               new(typeof(Country), new Guid("{C6030B2D-16F1-4854-9FAB-8A69B7FFAC6C}")),
                                                               new(typeof(Employee), new Guid("{AA46DA53-9B21-4DEC-9C70-720BDA1CB198}")),
                                                           ]));

        public IBssFrameworkSettings RegisterSupportLegacyServices()
        {
            return settings.SetSubscriptionAssembly(typeof(EmployeeUpdateSubscription).Assembly)
                           .SetNotificationDefaultMailSenderContainer<SampleSystemDefaultMailSenderContainer>()
                           .SetNotificationEmployee<Employee>()
                           .SetDTOMapping<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService, PersistentDomainObjectBase, EventDTOBase>();
        }

        public IBssFrameworkSettings AddQueryVisitors()
        {
            return settings.AddQueryVisitors<ExpressionVisitorContainerDomainIdentItem<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>()
                           .AddQueryVisitors<ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();
        }
    }
}
