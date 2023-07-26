using Framework.Authorization.Notification;
using Framework.Configuration.Core;
using Framework.Persistent;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class SecurityItemSourceSourceLambda : SecurityItemSourceLambdaMetadata<object, object, ManagementUnit>
{
    public SecurityItemSourceSourceLambda()
    {
        this.Lambda = (context, versions) => new List<ManagementUnit>();
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
    }

    public override NotificationExpandType ExpandType { get; protected set; } = NotificationExpandType.Direct;

    internal void SetFunc(Func<object, DomainObjectVersions<object>, IEnumerable<ManagementUnit>> lambda)
    {
        this.Lambda = lambda;
    }
}
