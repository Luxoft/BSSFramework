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
        private readonly IOperationEventListenerContainer<PersistentDomainObjectBase> operationListeners;

        private readonly ISampleSystemDTOMappingService mappingService;

        public SampleSystemEventsSubscriptionManager(
                IOperationEventListenerContainer<PersistentDomainObjectBase> operationListeners,
                [NotNull] IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender,
                [NotNull] ISampleSystemDTOMappingService mappingService)
            : base(operationListeners, messageSender)
        {
            this.operationListeners = operationListeners;
            this.mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<BusinessUnit>();
            this.SubscribeForSaveOperation<Employee>();
            this.SubscribeForSaveAndRemoveOperation<Information>();

            this.SubscribeCustom<Employee, BLLBaseOperation>(
                domainObject => true,
                operation => operation == BLLBaseOperation.Save,
                domainObject => new EmployeeCustomEventModelSaveEventDTO(this.mappingService, new EmployeeCustomEventModel(domainObject)));
        }

        private void SubscribeCustom<TDomainObject, TOperation>(
            Func<TDomainObject, bool> filter,
            Func<TOperation, bool> operationsFilter,
            Func<TDomainObject, EventDTOBase> convertFunc)
            where TDomainObject : PersistentDomainObjectBase
            where TOperation : struct, Enum
        {
            this.operationListeners.GetEventListener<TDomainObject, TOperation>().OperationProcessed += (_, eventArgs) =>
             {
                 if (filter(eventArgs.DomainObject) && operationsFilter(eventArgs.Operation))
                 {
                     var message = new DomainOperationSerializeData<TDomainObject, TOperation>
                     {
                         DomainObject = eventArgs.DomainObject,
                         Operation = eventArgs.Operation,
                         CustomSendObject = convertFunc(eventArgs.DomainObject)
                     };

                     this.MessageSender.Send(message);
                 }
             };
        }
    }
}
