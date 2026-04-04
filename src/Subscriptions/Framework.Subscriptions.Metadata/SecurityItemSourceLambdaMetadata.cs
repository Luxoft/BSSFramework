using SecuritySystem;
using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions.Metadata;

public abstract class SecurityLambdaMetadata<TDomainObject, TSecurityContext> : LambdaMetadata<TDomainObject, IEnumerable<TSecurityContext>>,
                                                                                ISecurityItemSourceLambdaMetadata

    where TDomainObject : class
    where TSecurityContext : ISecurityContext
{
    /// <inheritdoc />
    public virtual NotificationExpandType ExpandType { get; protected init; }

    public Type SecurityContextType { get; } = typeof(TSecurityContext);
}
