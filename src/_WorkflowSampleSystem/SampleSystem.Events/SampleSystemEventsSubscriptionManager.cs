using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Events;

using JetBrains.Annotations;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events
{
    public class SampleSystemEventsSubscriptionManager : EventsSubscriptionManagerBase<ISampleSystemBLLContext, PersistentDomainObjectBase>
    {
        private readonly ISampleSystemDTOMappingService mappingService;

        public SampleSystemEventsSubscriptionManager(ISampleSystemBLLContext context, [NotNull] IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender)
            : base(context, messageSender)
        {
            this.mappingService = new SampleSystemServerPrimitiveDTOMappingService(this.Context);
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
            this.Context.OperationListeners.GetEventListener<TDomainObject, TOperation>().OperationProcessed += (_, eventArgs) =>
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
