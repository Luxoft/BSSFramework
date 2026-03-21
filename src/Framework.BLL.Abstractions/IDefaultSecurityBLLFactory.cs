using Framework.Application.Domain;

using SecuritySystem;
using SecuritySystem.Providers;

namespace Framework.BLL;

public interface IDefaultSecurityBLLFactory<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(SecurityRule securityRule)
        where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
