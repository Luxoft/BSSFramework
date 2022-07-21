using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;

public class DBSessionEventListener : IDBSessionEventListener
{
    private readonly IInitializeManager initializeManager;

    private readonly IConfigurationBLLContext configurationBllContext;

    private readonly IStandardSubscriptionService subscriptionService;

    private readonly IReadOnlyCollection<IFlushedDALListener> flushedDalListener;

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener;

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener;


    public DBSessionEventListener(
            IInitializeManager initializeManager,
            IEnumerable<IFlushedDALListener> flushedDalListener,
            IEnumerable<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener,
            IEnumerable<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener,
            IConfigurationBLLContext configurationBLLContext,
            IStandardSubscriptionService subscriptionService)
    {
        this.initializeManager = initializeManager;
        this.configurationBllContext = configurationBLLContext;
        this.subscriptionService = subscriptionService;

        this.flushedDalListener = flushedDalListener.ToArray();
        this.beforeTransactionCompletedDalListener = beforeTransactionCompletedDalListener.ToArray();
        this.afterTransactionCompletedDalListener = afterTransactionCompletedDalListener.ToArray();
    }

    public void OnFlushed(DALChangesEventArgs eventArgs)
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        this.flushedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    public void OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        this.beforeTransactionCompletedDalListener.Concat(this.GetSubscriptionDALListeners()).Foreach(listener => listener.Process(eventArgs));
    }

    public void OnAfterTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        this.afterTransactionCompletedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    private IEnumerable<IBeforeTransactionCompletedDALListener> GetSubscriptionDALListeners()
    {
        return from targetSystemService in this.configurationBllContext.GetPersistentTargetSystemServices()

               where targetSystemService.TargetSystem.SubscriptionEnabled

               select new SubscriptionDALListener(targetSystemService, this.subscriptionService);
    }
}
