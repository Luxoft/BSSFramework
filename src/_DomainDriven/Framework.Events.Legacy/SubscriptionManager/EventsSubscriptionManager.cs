using CommonFramework;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events.Legacy;

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

    protected void SubscribeForSaveOperation<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.Subscribe<TDomainObject>(EventOperation.Save);
    }

    protected void SubscribeForSaveAndRemoveOperation<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.Subscribe<TDomainObject>(EventOperation.Save, EventOperation.Remove);
    }

    protected void Subscribe<TDomainObject>(EventOperation mainEventOperation, params EventOperation[] otherEventOperations)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var allEventOperations = new[] { mainEventOperation }.Concat(otherEventOperations).ToHashSet();

        this.Subscribe<TDomainObject>((_, operation) => allEventOperations.Contains(operation));
    }

    protected void Subscribe<TDomainObject>(
        Func<TDomainObject, EventOperation, bool> filter,
        Func<TDomainObject, EventOperation, object> customSendObjectConvertFunc = null)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var listener = new Listener<TDomainObject>
                       {
                           Filter = filter,
                           CreateMessage = (domainObject, operation) => new DomainOperationSerializeData<TDomainObject>
                                                                        {
                                                                            DomainObject = domainObject,
                                                                            Operation = operation,
                                                                            CustomSendObject = customSendObjectConvertFunc?.Invoke(domainObject, operation)
                                                                        }
                       };

        this.sc.AddSingleton(listener);
    }

    private IServiceProvider BuildCache()
    {
        this.Subscribe();

        return this.sc.BuildServiceProvider();
    }

    private void Receive<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.cache.Value.GetRequiredService<IEnumerable<Listener<TDomainObject>>>().Foreach(
            listener =>
            {
                if (listener.Filter(domainObject, domainObjectEvent))
                {
                    var message = listener.CreateMessage(domainObject, domainObjectEvent);

                    this.MessageSender.Send(message);
                }
            });
    }

    void IEventOperationReceiver.Receive<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
    {
        if (domainObject is TPersistentDomainObjectBase)
        {
            new Action<TPersistentDomainObjectBase, EventOperation>(this.Receive)
                .CreateGenericMethod(typeof(TDomainObject))
                .Invoke(this, [domainObject, domainObjectEvent]);
        }
    }

    private class Listener<TDomainObject>
    {
        public Func<TDomainObject, EventOperation, bool> Filter { get; init; }

        public Func<TDomainObject, EventOperation, DomainOperationSerializeData<TDomainObject>> CreateMessage { get; init; }
    }
}
