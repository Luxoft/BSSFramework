using System;

using Framework.Core;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.Events
{
    /// <summary>
    /// Класс для описания правил подписок на доменные евенты
    /// </summary>
    /// <typeparam name="TBLLContext"></typeparam>
    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    public abstract class EventsSubscriptionManagerBase<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<TBLLContext>, IEventsSubscriptionManager<TBLLContext, TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
        where TBLLContext : class, IBLLOperationEventContext<TPersistentDomainObjectBase>
    {
        protected EventsSubscriptionManagerBase(TBLLContext context, [NotNull] IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>> messageSender)
            : base(context)
        {
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
            this.Context.OperationListeners.GetEventListener<TDomainObject, TOperation>().OperationProcessed += (_, eventArgs) =>
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
