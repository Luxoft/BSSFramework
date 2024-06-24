using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLContext<in TPersistentDomainObjectBase, TIdent> :

    IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
        where TDomainObject : TPersistentDomainObjectBase;
}


public interface ISecurityBLLContext<out TAuthorizationBLLContext, in TPersistentDomainObjectBase, TIdent> : ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>,

    IAuthorizationBLLContextContainer<TAuthorizationBLLContext>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
}
