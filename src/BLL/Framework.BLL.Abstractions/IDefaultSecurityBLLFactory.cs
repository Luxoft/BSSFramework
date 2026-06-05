using Anch.SecuritySystem;
using Anch.SecuritySystem.Providers;

using Framework.Application.Domain;

namespace Framework.BLL;

public interface IDefaultSecurityBLLFactory<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(SecurityRule securityRule)
        where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
        where TDomainObject : class, TPersistentDomainObjectBase;
}

