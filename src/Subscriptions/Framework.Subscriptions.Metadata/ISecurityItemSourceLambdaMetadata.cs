using System.Collections.Immutable;

using SecuritySystem;
using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions.Metadata;

public record TypedNotificationFilterGroup<TSecurityContext> : NotificationFilterGroup
    where TSecurityContext : ISecurityContext
{
    public required ImmutableArray<TSecurityContext> SecurityContextList { get; init; }
}
