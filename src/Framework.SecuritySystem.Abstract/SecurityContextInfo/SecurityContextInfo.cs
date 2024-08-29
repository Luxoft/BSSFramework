using Framework.Persistent;

namespace Framework.SecuritySystem;

public abstract record SecurityContextInfo(Guid Id, string Name)
{
    public abstract Type Type { get; }
}

public record SecurityContextInfo<TSecurityContext>(Guid Id, string Name) : SecurityContextInfo(Id, Name)
    where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
{
    public override Type Type { get; } = typeof(TSecurityContext);
}
