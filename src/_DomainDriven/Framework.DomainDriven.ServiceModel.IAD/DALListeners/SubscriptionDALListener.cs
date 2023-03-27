using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.ServiceModel.Subscriptions;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class SubscriptionDALListener : IBeforeTransactionCompletedDALListener
{
    private readonly IPersistentTargetSystemService targetSystemService;

    private readonly IStandardSubscriptionService subscriptionService;

    public SubscriptionDALListener([NotNull] IPersistentTargetSystemService targetSystemService, [NotNull] IStandardSubscriptionService subscriptionService)
    {
        this.targetSystemService = targetSystemService ?? throw new ArgumentNullException(nameof(targetSystemService));
        this.subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
    }

    public void Process(DALChangesEventArgs eventArgs)
    {
        if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

        if (!this.targetSystemService.TargetSystem.SubscriptionEnabled)
        {
            return;
        }

        this.targetSystemService.SubscriptionService.GetObjectModifications(eventArgs.Changes).Foreach(info => this.subscriptionService.ProcessChanged(new ObjectModificationInfoDTO<Guid>(info)));
    }
}
