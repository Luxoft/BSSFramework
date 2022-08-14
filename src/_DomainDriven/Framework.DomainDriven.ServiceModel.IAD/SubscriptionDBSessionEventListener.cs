using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class SubscriptionDBSessionEventListener : IDBSessionEventListener
{
    private readonly IInitializeManager initializeManager;

    private readonly IConfigurationBLLContext configurationBllContext;

    private readonly IStandardSubscriptionService subscriptionService;


    public SubscriptionDBSessionEventListener(
            IInitializeManager initializeManager,
            IConfigurationBLLContext configurationBLLContext,
            IStandardSubscriptionService subscriptionService)
    {
        this.initializeManager = initializeManager;
        this.configurationBllContext = configurationBLLContext;
        this.subscriptionService = subscriptionService;

    }

    public void OnFlushed(DALChangesEventArgs eventArgs)
    {
    }

    public void OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        this.GetSubscriptionDALListeners().Foreach(listener => listener.Process(eventArgs));
    }

    public void OnAfterTransactionCompleted(DALChangesEventArgs eventArgs)
    {
    }

    private IEnumerable<IBeforeTransactionCompletedDALListener> GetSubscriptionDALListeners()
    {
        return from targetSystemService in this.configurationBllContext.GetPersistentTargetSystemServices()

               where targetSystemService.TargetSystem.SubscriptionEnabled

               select new SubscriptionDALListener(targetSystemService, this.subscriptionService);
    }
}
