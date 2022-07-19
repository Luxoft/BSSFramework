using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

public class EventSubscriber : IEventSubscriber, IDisposable
{
    private readonly IInitializeManager initializeManager;

    private readonly IDBSession session;

    private readonly IConfigurationBLLContext configurationBllContext;

    private readonly IStandardSubscriptionService subscriptionService;

    private readonly IEnumerable<IEventsSubscriptionManager> eventsSubscriptionManagers;

    private readonly IReadOnlyCollection<IFlushedDALListener> flushedDALListener;

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener;

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener;

    private bool subscribed;

    public EventSubscriber(
            IInitializeManager initializeManager,
            IDBSession session,
            IEnumerable<IFlushedDALListener> flushedDALListener,
            IEnumerable<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDALListener,
            IEnumerable<IAfterTransactionCompletedDALListener> afterTransactionCompletedDALListener,
            IEnumerable<IEventsSubscriptionManager> eventsSubscriptionManagers,
            IConfigurationBLLContext configurationBLLContext,
            IStandardSubscriptionService subscriptionService)
    {
        this.initializeManager = initializeManager;
        this.session = session;
        this.configurationBllContext = configurationBLLContext;
        this.subscriptionService = subscriptionService;

        this.flushedDALListener = flushedDALListener.ToArray();
        this.beforeTransactionCompletedDalListener = beforeTransactionCompletedDALListener.ToArray();
        this.afterTransactionCompletedDalListener = afterTransactionCompletedDALListener.ToArray();

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
        this.session.Flushed += (_, eventArgs) => this.flushedDALListener.Foreach(listener => listener.Process(eventArgs));

        this.session.BeforeTransactionCompleted += (_, eventArgs) => this.beforeTransactionCompletedDalListener.Concat(this.GetSubscriptionDALListeners()).Foreach(listener => listener.Process(eventArgs));

        this.session.AfterTransactionCompleted += (_, eventArgs) => this.afterTransactionCompletedDalListener.Foreach(listener => listener.Process(eventArgs));

        this.eventsSubscriptionManagers.Foreach(eventsSubscriptionManager => eventsSubscriptionManager.Subscribe());
    }

    private IEnumerable<IBeforeTransactionCompletedDALListener> GetSubscriptionDALListeners()
    {
        return from targetSystemService in this.configurationBllContext.GetPersistentTargetSystemServices()

               where targetSystemService.TargetSystem.SubscriptionEnabled

               select new SubscriptionDALListener(targetSystemService, this.subscriptionService);
    }

    public void Dispose()
    {
    }
}
