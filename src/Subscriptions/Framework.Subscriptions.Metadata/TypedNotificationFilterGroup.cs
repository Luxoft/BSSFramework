using System.Collections.Immutable;

using SecuritySystem;
using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions.Metadata;

public abstract record TypedNotificationFilterGroup
{
    public abstract Type SecurityContextType { get; }
}

public record TypedNotificationFilterGroup<TSecurityContext> : TypedNotificationFilterGroup
    where TSecurityContext : ISecurityContext
{
    public override Type SecurityContextType { get; } = typeof(TSecurityContext);

    public required ImmutableArray<TSecurityContext> SecurityContextList { get; init; }

    public required NotificationExpandType ExpandType { get; init; }
}
