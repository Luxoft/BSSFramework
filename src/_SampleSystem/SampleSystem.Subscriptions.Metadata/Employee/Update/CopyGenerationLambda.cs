using Framework.Configuration.Core;
using Framework.Notification;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public sealed class CopyGenerationLambda : GenerationLambdaBase<Domain.Employee>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CopyGenerationLambda"/> class.
    /// </summary>
    public CopyGenerationLambda()
    {
        this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Update;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            ISampleSystemBLLContext context,
            DomainObjectVersions<Domain.Employee> versions)
    {
        return new[] { new NotificationMessageGenerationInfo("tester@luxoft.com", versions.Previous, versions.Current) };
    }
}
