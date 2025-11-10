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
        Func<TDomainObject, EventOperation, object>? customSendObjectConvertFunc = null)
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

        return this.sc.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }

    private async Task Receive<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken cancellationToken)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        foreach (var listener in this.cache.Value.GetRequiredService<IEnumerable<Listener<TDomainObject>>>())
        {
            if (listener.Filter(domainObject, domainObjectEvent))
            {
                var message = listener.CreateMessage(domainObject, domainObjectEvent);

                await this.MessageSender.SendAsync(message, cancellationToken);
            }
        }
    }

    async Task IEventOperationReceiver.Receive<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken cancellationToken)
    {
        if (domainObject is TPersistentDomainObjectBase)
        {
            await new Func<TPersistentDomainObjectBase, EventOperation, CancellationToken, Task>(this.Receive)
                  .CreateGenericMethod(typeof(TDomainObject))
                  .Invoke<Task>(this, [domainObject, domainObjectEvent, cancellationToken]);
        }
    }

    private class Listener<TDomainObject>
    {
        public required Func<TDomainObject, EventOperation, bool> Filter { get; init; }

        public required Func<TDomainObject, EventOperation, DomainOperationSerializeData<TDomainObject>> CreateMessage { get; init; }
    }
}
