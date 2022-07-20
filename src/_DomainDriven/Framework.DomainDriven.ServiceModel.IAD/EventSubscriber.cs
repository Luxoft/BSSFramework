using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

public class EventSubscriberManager
{
    private readonly IServiceProvider serviceProvider;

    private readonly Lazy<IEventSubscriber> lazyEventSubscriber;

    private readonly IDictionaryCache<Type, object> operationEventListenerContainers;

    public EventSubscriberManager(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        this.lazyEventSubscriber = new Lazy<IEventSubscriber>(() =>
        {
            var eventSubscriber = this.serviceProvider.GetRequiredService<IEventSubscriber>();

            eventSubscriber.Subscribe();

            return eventSubscriber;
        });

        this.DBSessionEventListener = LazyInterfaceImplementHelper.CreateProxy<IDBSessionEventListener>(() => this.lazyEventSubscriber.Value);

        this.operationEventListenerContainers = new LazyImplementDictionaryCache<Type, object>(t =>
        {
            this.lazyEventSubscriber.Value.Subscribe();

            return this.serviceProvider.GetRequiredService(typeof(OperationEventListenerContainer<>).MakeGenericType(t));
        }).WithLock();
    }

    public IOperationEventListenerContainer<TPersistentDomainObjectBase> GetOperationEventListenerContainer<TPersistentDomainObjectBase>()
    {
        return (IOperationEventListenerContainer<TPersistentDomainObjectBase>)this.operationEventListenerContainers[typeof(TPersistentDomainObjectBase)];
    }

    public IDBSessionEventListener DBSessionEventListener { get; }

    public bool HasListeners => (this.DBSessionEventListener as Lazy<IDBSessionEventListener>).IsValueCreated;

    public void TryCloseDbSession()
    {
        if (this.HasListeners)
        {
            this.serviceProvider.GetRequiredService<IDBSession>().Close();
        }
    }
}

public class EventSubscriber : IEventSubscriber
{
    private readonly IInitializeManager initializeManager;

    private readonly IConfigurationBLLContext configurationBllContext;

    private readonly IStandardSubscriptionService subscriptionService;

    private readonly IEnumerable<IEventsSubscriptionManager> eventsSubscriptionManagers;

    private readonly IReadOnlyCollection<IFlushedDALListener> flushedDalListener;

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener;

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener;

    private bool subscribed;

    public EventSubscriber(
            IInitializeManager initializeManager,
            IEnumerable<IFlushedDALListener> flushedDalListener,
            IEnumerable<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener,
            IEnumerable<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener,
            IEnumerable<IEventsSubscriptionManager> eventsSubscriptionManagers,
            IConfigurationBLLContext configurationBLLContext,
            IStandardSubscriptionService subscriptionService)
    {
        this.initializeManager = initializeManager;
        this.configurationBllContext = configurationBLLContext;
        this.subscriptionService = subscriptionService;

        this.flushedDalListener = flushedDalListener.ToArray();
        this.beforeTransactionCompletedDalListener = beforeTransactionCompletedDalListener.ToArray();
        this.afterTransactionCompletedDalListener = afterTransactionCompletedDalListener.ToArray();

        this.eventsSubscriptionManagers = eventsSubscriptionManagers.ToArray();
    }

    /// <inheritdoc />
    public void Subscribe()
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        if (this.subscribed)
        {
            return;
        }

        this.subscribed = true;

        this.InternalSubscribe();
    }


    protected virtual void InternalSubscribe()
    {
        this.eventsSubscriptionManagers.Foreach(eventsSubscriptionManager => eventsSubscriptionManager.Subscribe());
    }

    public void OnFlushed(DALChangesEventArgs eventArgs)
    {
        this.flushedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    public void OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        this.beforeTransactionCompletedDalListener.Concat(this.GetSubscriptionDALListeners()).Foreach(listener => listener.Process(eventArgs));
    }

    public void OnAfterTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        this.afterTransactionCompletedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    private IEnumerable<IBeforeTransactionCompletedDALListener> GetSubscriptionDALListeners()
    {
        return from targetSystemService in this.configurationBllContext.GetPersistentTargetSystemServices()

               where targetSystemService.TargetSystem.SubscriptionEnabled

               select new SubscriptionDALListener(targetSystemService, this.subscriptionService);
    }
}
