using Framework.BLL.Events;
using Framework.Infrastructure.DALListeners;
using Framework.Infrastructure.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;
using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public static class SampleSystemFrameworkExtensions
{
    extension(IBssFrameworkSetup settings)
    {
        public IBssFrameworkSetup AddNamedLocks() =>
            settings.AddNamedLocks(s => s
                                        .SetNameLockType<GenericNamedLock>(nl => nl.Name)
                                        .AddManual(typeof(BusinessUnitAncestorLink))
                                        .AddManual(typeof(ManagementUnitAncestorLink))
                                        .AddManual(typeof(LocationAncestorLink)));

        public IBssFrameworkSetup AddListeners() =>
            settings.AddListener<SubscriptionDALListener>()
                    .AddListener<ExampleFaultDALListener>()
                    .AddListener<FixDomainObjectEventRevisionNumberDALListener>()
                    .AddListener<DependencyDetailEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>();

        public IBssFrameworkSetup AddSubscriptionManagers() =>
            settings.AddSubscriptionManager<ExampleSampleSystemEventsSubscriptionManager>()
                    .AddSubscriptionManager<ExampleSampleSystemAribaEventsSubscriptionManager>();

        public IBssFrameworkSetup AddBLLSystem() => settings.AddBLLSystem<ISampleSystemBLLContext, SampleSystemBLLContext>();

        public IBssFrameworkSetup AddConfigurationSystemConstants() =>
            settings.AddSystemConstant(typeof(SampleSystemSystemConstant));

        public IBssFrameworkSetup AddConfigurationTargetSystems() =>
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

        public IBssFrameworkSetup AddSupportLegacyServices() =>
            settings.SetSubscriptionAssembly(typeof(EmployeeUpdateSubscription).Assembly)
                    .SetNotificationDefaultMailSenderContainer<SampleSystemDefaultMailSenderContainer>()
                    .SetNotificationEmployee<Employee>()
                    .SetDTOMapping<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService, PersistentDomainObjectBase, EventDTOBase>();

        //public IBssFrameworkBuilder AddQueryVisitors() =>
        //    settings.AddQueryVisitors<ExpressionVisitorContainerDomainIdentItem<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>()
        //            .AddQueryVisitors<ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();
    }
}
