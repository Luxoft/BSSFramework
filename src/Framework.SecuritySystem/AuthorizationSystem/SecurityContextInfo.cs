using Framework.Persistent;

namespace Framework.SecuritySystem;

public interface ISecurityContextInfo
{
    Type Type { get; }

    string Name { get; }

    Guid Id { get; }
}

public record SecurityContextInfo<TSecurityContext>(Guid Id, string Name) : ISecurityContextInfo
    where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
{
    public Type Type { get; } = typeof(TSecurityContext);
}
