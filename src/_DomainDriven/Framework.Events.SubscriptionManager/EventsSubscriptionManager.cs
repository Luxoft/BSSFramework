using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events;

/// <summary>
/// Класс для описания правил подписок на доменные евенты
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public abstract class EventsSubscriptionManager<TPersistentDomainObjectBase> : IEventOperationReceiver
    where TPersistentDomainObjectBase : class
{
    private readonly IServiceCollection sc = new ServiceCollection();

    private readonly Lazy<IServiceProvider> cache;

    protected EventsSubscriptionManager(IEventDTOMessageSender<TPersistentDomainObjectBase> messageSender)
    {
        this.MessageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));

        this.cache = new Lazy<IServiceProvider>(this.BuildCache);
    }

    protected IEventDTOMessageSender<TPersistentDomainObjectBase> MessageSender { get; }

    public abstract void Subscribe();

    protected void SubscribeForSaveOperation<T>()
        where T : class, TPersistentDomainObjectBase
    {
        this.Subscribe<T>(z => true, z => z == DomainObjectEvent.Save);
    }

    protected void SubscribeForSaveAndRemoveOperation<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.Subscribe<TDomainObject>(z => true, z => z == DomainObjectEvent.Save || z == DomainObjectEvent.Remove);
    }

    protected void Subscribe<TDomainObject>(
            Func<TDomainObject, bool> domainObjectFilter,
            Func<DomainObjectEvent, bool> operationFilter)
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
            Func<DomainObjectEvent, bool> operationFilter,
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

    private void Receive<TDomainObject>(TDomainObject domainObject, DomainObjectEvent domainObjectEvent)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.cache.Value.GetRequiredService<IEnumerable<Listener<TDomainObject>>>().Foreach(listener =>
        {
            if (listener.Filter(domainObject, domainObjectEvent))
            {
                var message = listener.CreateMessage(domainObject, domainObjectEvent);

                this.MessageSender.Send(message);
            }
        });
    }

    void IEventOperationReceiver.Receive<TDomainObject>(TDomainObject domainObject, DomainObjectEvent domainObjectEvent)
    {
        new Action<TPersistentDomainObjectBase, DomainObjectEvent>(this.Receive)
            .CreateGenericMethod(typeof(TDomainObject))
            .Invoke(this, [domainObject, domainObjectEvent]);
    }

    private class Listener<TDomainObject>
    {
        public Func<TDomainObject, DomainObjectEvent, bool> Filter { get; init; }

        public Func<TDomainObject, DomainObjectEvent, DomainOperationSerializeData<TDomainObject>> CreateMessage { get; init; }
    }
}
