using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

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

    protected EventsSubscriptionManagerBase([NotNull] IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>> messageSender)
    {
        this.MessageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));

        this.cache = new Lazy<IServiceProvider>(this.BuildCache);
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

    protected void Subscribe<TDomainObject>(Func<TDomainObject, bool> domainObjectFilter, Func<BLLBaseOperation, bool> operationFilter)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        this.Subscribe<TDomainObject, BLLBaseOperation>(domainObjectFilter, operationFilter);
    }

    protected void Subscribe<TDomainObject, TOperation>(
            Func<TDomainObject, bool> domainObjectFilter,
            Func<TOperation, bool> operationFilter)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
    {
        var listener = new Listener<TDomainObject, TOperation>()
                       {
                               Filter = (domainObject, operation) => domainObjectFilter(domainObject) && operationFilter(operation),
                               CreateMessage = (domainObject, operation) => new DomainOperationSerializeData<TDomainObject, TOperation>
                                                                            {
                                                                                    DomainObject = domainObject,
                                                                                    Operation = operation
                                                                            }
                       };

        this.sc.AddSingleton(listener);
    }

    protected void SubscribeCustom<TDomainObject, TOperation>(
            Func<TDomainObject, bool> domainObjectFilter,
            Func<TOperation, bool> operationFilter,
            Func<TDomainObject, object> convertFunc)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
    {
        var listener = new Listener<TDomainObject, TOperation>()
                       {
                               Filter = (domainObject, operation) => domainObjectFilter(domainObject) && operationFilter(operation),
                               CreateMessage = (domainObject, operation) => new DomainOperationSerializeData<TDomainObject, TOperation>
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

    public virtual void OnFired<TDomainObject, TOperation>(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
    {
        this.cache.Value.GetRequiredService<IEnumerable<Listener<TDomainObject, TOperation>>>().Foreach(listener =>
        {
            if (listener.Filter(eventArgs.DomainObject, eventArgs.Operation))
            {
                var message = listener.CreateMessage(eventArgs.DomainObject, eventArgs.Operation);

                this.MessageSender.Send(message);
            }
        });
    }

    private class Listener<TDomainObject, TOperation>
            where TDomainObject : class
            where TOperation : struct, Enum
    {
        public Func<TDomainObject, TOperation, bool> Filter { get; set; }

        public Func<TDomainObject, TOperation, DomainOperationSerializeData<TDomainObject, TOperation>> CreateMessage { get; set; }
    }
}
