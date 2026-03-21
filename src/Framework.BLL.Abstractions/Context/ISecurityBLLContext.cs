using Framework.Application.Domain;
using Framework.BLL.Context._Authorization;

using SecuritySystem.Providers;

namespace Framework.BLL.Context;

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
