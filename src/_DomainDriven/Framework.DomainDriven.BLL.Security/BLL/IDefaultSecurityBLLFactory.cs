using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface IDefaultSecurityBLLFactory<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(SecurityRule securityRule)
        where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
