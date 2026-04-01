using Framework.BLL.DTOMapping.Domain;
using Framework.Configuration.BLL;
using Framework.Database;
using Framework.Database.DALListener;
using Framework.Infrastructure.SubscriptionService;

namespace Framework.Infrastructure.DALListeners;

public class SubscriptionDALListener(IConfigurationBLLContext configurationBllContext, IStandardSubscriptionService subscriptionService)
    : IBeforeTransactionCompletedDALListener
{
    public async Task Process(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        foreach (var targetSystemService in configurationBllContext.GetTargetSystemServices())
        {
            if (targetSystemService.TargetSystem.SubscriptionEnabled)
            {
                foreach (var info in targetSystemService.SubscriptionService.GetObjectModifications(eventArgs.Changes))
                {
                    subscriptionService.ProcessChanged(new ObjectModificationInfoDTO<Guid>(info));
                }
            }
        }
    }
}