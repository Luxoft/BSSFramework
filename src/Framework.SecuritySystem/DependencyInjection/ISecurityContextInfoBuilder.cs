using Framework.Persistent;

namespace Framework.SecuritySystem.DependencyInjection;

public interface ISecurityContextInfoBuilder<in TIdent>
{
    ISecurityContextInfoBuilder<TIdent> Add<TSecurityContext>(TIdent ident, string? name = null, Func<TSecurityContext, string>? displayFunc = null)
        where TSecurityContext : ISecurityContext, IIdentityObject<TIdent>;
}
