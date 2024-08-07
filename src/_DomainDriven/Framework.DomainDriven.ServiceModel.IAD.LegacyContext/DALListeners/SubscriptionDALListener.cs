using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.Subscriptions;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class SubscriptionDALListener : IBeforeTransactionCompletedDALListener
{
    private readonly IConfigurationBLLContext configurationBllContext;

    private readonly IStandardSubscriptionService subscriptionService;

    public SubscriptionDALListener(
        IConfigurationBLLContext configurationBLLContext,
        IStandardSubscriptionService subscriptionService)
    {
        this.configurationBllContext = configurationBLLContext;
        this.subscriptionService = subscriptionService;
    }

    public void Process(DALChangesEventArgs eventArgs)
    {
        foreach (var targetSystemService in this.configurationBllContext.GetTargetSystemServices())
        {
            if (targetSystemService.TargetSystem.SubscriptionEnabled)
            {
                foreach (var info in targetSystemService.SubscriptionService.GetObjectModifications(eventArgs.Changes))
                {
                    this.subscriptionService.ProcessChanged(new ObjectModificationInfoDTO<Guid>(info));
                }
            }
        }
    }
}
