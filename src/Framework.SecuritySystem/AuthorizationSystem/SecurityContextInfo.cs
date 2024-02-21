using Framework.Persistent;

namespace Framework.SecuritySystem;

public interface ISecurityContextInfo
{
    Type Type { get; }

    string Name { get; }
}

public interface ISecurityContextInfo<out TIdent> : ISecurityContextInfo
{
    TIdent Id { get; }
}

public record SecurityContextInfo<TSecurityContext, TIdent>(TIdent Id, string Name) : ISecurityContextInfo<TIdent>
    where TSecurityContext : ISecurityContext, IIdentityObject<TIdent>
{
    public Type Type { get; } = typeof(TSecurityContext);
}
