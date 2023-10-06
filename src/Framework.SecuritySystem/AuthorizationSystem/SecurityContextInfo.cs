using Framework.Persistent;

namespace Framework.SecuritySystem;

public record SecurityContextInfo(Type Type, string Name);

public record SecurityContextInfo<TSecurityContext, TIdent>(TIdent Id, string Name)
    : SecurityContextInfo(typeof(TSecurityContext), Name)
    where TSecurityContext : ISecurityContext, IIdentityObject<TIdent>;
