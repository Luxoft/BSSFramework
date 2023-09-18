using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLContext<out TAuthorizationBLLContext, in TPersistentDomainObjectBase, TIdent> :

    IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>,
    IAuthorizationBLLContextContainer<TAuthorizationBLLContext>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TAuthorizationBLLContext : IAuthorizationBLLContext<TIdent>
{
}
