using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Events;

using JetBrains.Annotations;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events
{
    public class SampleSystemEventsSubscriptionManager : EventsSubscriptionManagerBase<PersistentDomainObjectBase>
    {
        private readonly ISampleSystemDTOMappingService mappingService;

        public SampleSystemEventsSubscriptionManager(
                [NotNull] IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender,
                [NotNull] ISampleSystemDTOMappingService mappingService)
            : base(messageSender)
        {
            this.mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<BusinessUnit>();
            this.SubscribeForSaveOperation<Employee>();
            this.SubscribeForSaveAndRemoveOperation<Information>();

            this.SubscribeCustom<Employee, BLLBaseOperation>(
                _ => true,
                operation => operation == BLLBaseOperation.Save,
                domainObject => new EmployeeCustomEventModelSaveEventDTO(this.mappingService, new EmployeeCustomEventModel(domainObject)));
        }
    }
}
