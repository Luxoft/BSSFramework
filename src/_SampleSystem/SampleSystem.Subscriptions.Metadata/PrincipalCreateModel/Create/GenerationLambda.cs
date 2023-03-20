using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Configuration.Core;
using Framework.Configuration.SubscriptionModeling;
using Framework.Notification;

namespace SampleSystem.Subscriptions.Metadata.PrincipalCreateModel.Create;

public sealed class GenerationLambda : LambdaMetadata<IAuthorizationBLLContext, Framework.Authorization.Domain.PrincipalCreateModel, IEnumerable<NotificationMessageGenerationInfo>>
{
    public GenerationLambda()
    {
        this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Create;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            IAuthorizationBLLContext context,
            DomainObjectVersions<Framework.Authorization.Domain.PrincipalCreateModel> versions)
    {
        return new[] { new NotificationMessageGenerationInfo("tester@luxoft.com", versions.Previous, versions.Current) };
    }
}
