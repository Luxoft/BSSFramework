using Framework.Application.Domain;

using SecuritySystem.Providers;

namespace Framework.BLL;

public interface IDefaultSecurityDomainBLLBase<in TPersistentDomainObjectBase, TDomainObject, TIdent> : IDefaultDomainBLLBase<
    TPersistentDomainObjectBase, TDomainObject, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
{
    ISecurityProvider<TDomainObject> SecurityProvider { get; }

    void CheckAccess(TDomainObject domainObject);
}

public interface IDefaultSecurityDomainBLLBase<out TBLLContext, in TPersistentDomainObjectBase, TDomainObject, TIdent> :
    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>,
    IDefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>;
