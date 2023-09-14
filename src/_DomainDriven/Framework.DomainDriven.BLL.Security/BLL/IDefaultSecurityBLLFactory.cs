using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface IDefaultSecurityBLLFactory<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
            where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(SecurityOperation securityOperation)
            where TDomainObject : class, TPersistentDomainObjectBase;
}
