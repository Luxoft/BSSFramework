using Framework.BLL.DTOMapping.Domain;
using Framework.Configuration.BLL;
using Framework.Database;
using Framework.Database.DALListener;
using Framework.Infrastructure.SubscriptionService;

namespace Framework.Infrastructure.DALListeners;

public class SubscriptionDALListener(IConfigurationBLLContext configurationBllContext, IObjectModificationProcessor subscriptionService)
    : IBeforeTransactionCompletedDALListener
{
    public async Task Process(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        foreach (var targetSystemService in configurationBllContext.GetTargetSystemServices())
        {
            if (targetSystemService.TargetSystem.SubscriptionEnabled)
            {
                foreach (var info in targetSystemService.GetObjectModifications(eventArgs.Changes))
                {
                    await subscriptionService.ProcessChanged(new ObjectModificationInfoDTO<Guid>(info), cancellationToken);
                }
            }
        }
    }
}
