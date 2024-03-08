using Framework.Core;
using Framework.DomainDriven;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events;

/// <summary>
/// Класс для описания правил подписок на доменные евенты
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public abstract class EventsSubscriptionManagerBase<TPersistentDomainObjectBase> : IOperationEventListener<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
{
    private readonly IServiceCollection sc = new ServiceCollection();

    private readonly Lazy<IServiceProvider> cache;

    protected EventsSubscriptionManagerBase(IEventDTOMessageSender<TPersistentDomainObjectBase> messageSender)
    {
        this.MessageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));

        this.cache = new Lazy<IServiceProvider>(this.BuildCache);
    }

    protected IEventDTOMessageSender<TPersistentDomainObjectBase> MessageSender { get; }

    /// <inheritdoc />
    public abstract void Subscribe();

    protected void SubscribeForSaveOperation<T>()
        where T : class, TPersistentDomainObjectBase
    {
        this.Subscribe<T>(z => true, z => z == EventOperation.Save);
    }

    protected void SubscribeForSaveAndRemoveOperation<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.Subscribe<TDomainObject>(z => true, z => z == EventOperation.Save || z == EventOperation.Remove);
    }

    protected void Subscribe<TDomainObject>(
            Func<TDomainObject, bool> domainObjectFilter,
            Func<EventOperation, bool> operationFilter)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        var listener = new Listener<TDomainObject>()
        {
            Filter = (domainObject, operation) => domainObjectFilter(domainObject) && operationFilter(operation),
            CreateMessage = (domainObject, operation) => new DomainOperationSerializeData<TDomainObject>
            {
                DomainObject = domainObject,
                Operation = operation
            }
        };

        this.sc.AddSingleton(listener);
    }

    protected void SubscribeCustom<TDomainObject>(
            Func<TDomainObject, bool> domainObjectFilter,
            Func<EventOperation, bool> operationFilter,
            Func<TDomainObject, object> convertFunc)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        var listener = new Listener<TDomainObject>()
        {
            Filter = (domainObject, operation) => domainObjectFilter(domainObject) && operationFilter(operation),
            CreateMessage = (domainObject, operation) => new DomainOperationSerializeData<TDomainObject>
            {
                DomainObject = domainObject,
                Operation = operation,
                CustomSendObject = convertFunc(domainObject)
            }
        };

        this.sc.AddSingleton(listener);
    }

    private IServiceProvider BuildCache()
    {
        this.Subscribe();

        return this.sc.BuildServiceProvider();
    }

    public virtual void OnFired<TDomainObject>(IDomainOperationEventArgs<TDomainObject> eventArgs)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.cache.Value.GetRequiredService<IEnumerable<Listener<TDomainObject>>>().Foreach(listener =>
        {
            if (listener.Filter(eventArgs.DomainObject, eventArgs.Operation))
            {
                var message = listener.CreateMessage(eventArgs.DomainObject, eventArgs.Operation);

                this.MessageSender.Send(message);
            }
        });
    }

    private class Listener<TDomainObject>
        where TDomainObject : class
    {
        public Func<TDomainObject, EventOperation, bool> Filter { get; set; }

        public Func<TDomainObject, EventOperation, DomainOperationSerializeData<TDomainObject>> CreateMessage { get; set; }
    }
}
