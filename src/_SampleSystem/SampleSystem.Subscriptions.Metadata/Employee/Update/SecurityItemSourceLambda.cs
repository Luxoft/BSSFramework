using Framework.Subscriptions;

using SampleSystem.Domain;

using SecuritySystem.Notification.Domain;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public sealed class SecurityItemSourceLambda
        : SecurityItemSourceLambdaBase<Domain.Employee, ManagementUnit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityItemSourceLambda"/> class.
    /// </summary>
    public SecurityItemSourceLambda()
    {
        this.ExpandType = NotificationExpandType.All;
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = (context, versions) => [];
    }
}
