using System;

using Framework.Core;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.Events
{
    /// <summary>
    /// Класс для описания правил подписок на доменные евенты
    /// </summary>
    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    public abstract class EventsSubscriptionManagerBase<TPersistentDomainObjectBase> : IEventsSubscriptionManager
        where TPersistentDomainObjectBase : class
    {
        private readonly IOperationEventListenerContainer<TPersistentDomainObjectBase> operationListeners;

        protected EventsSubscriptionManagerBase(IOperationEventListenerContainer<TPersistentDomainObjectBase> operationListeners, [NotNull] IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>> messageSender)
        {
            this.operationListeners = operationListeners;
            this.MessageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        protected IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>> MessageSender { get; }

        /// <inheritdoc />
        public abstract void Subscribe();

        protected void SubscribeForSaveOperation<T>()
            where T : class, TPersistentDomainObjectBase
        {
            this.Subscribe<T>(z => true, z => z == BLLBaseOperation.Save);
        }

        protected void SubscribeForSaveAndRemoveOperation<TDomainObject>()
           where TDomainObject : class, TPersistentDomainObjectBase
        {
            this.Subscribe<TDomainObject>(z => true, z => z == BLLBaseOperation.Save || z == BLLBaseOperation.Remove);
        }

        protected void Subscribe<TDomainObject>(Func<TDomainObject, bool> filter, Func<BLLBaseOperation, bool> operationsFilter)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            this.Subscribe<TDomainObject, BLLBaseOperation>(filter, operationsFilter);
        }

        protected void Subscribe<TDomainObject, TOperation>(Func<TDomainObject, bool> filter, Func<TOperation, bool> operationsFilter)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
        {
            this.operationListeners.GetEventListener<TDomainObject, TOperation>().OperationProcessed += (_, eventArgs) =>
            {
                if (filter(eventArgs.DomainObject) && operationsFilter(eventArgs.Operation))
                {
                    var message = new DomainOperationSerializeData<TDomainObject, TOperation>
                    {
                        DomainObject = eventArgs.DomainObject,
                        Operation = eventArgs.Operation
                    };

                    this.MessageSender.Send(message);
                }
            };
        }
    }
}
