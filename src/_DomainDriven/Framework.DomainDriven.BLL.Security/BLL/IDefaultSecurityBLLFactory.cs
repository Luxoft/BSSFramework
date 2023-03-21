using System;

using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface IDefaultSecurityBLLFactory<in TPersistentDomainObjectBase, TSecurityOperationCode, TIdent> : IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TSecurityOperationCode : struct, Enum
{
    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
            where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(TSecurityOperationCode securityOperationCode)
            where TDomainObject : class, TPersistentDomainObjectBase;

    IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(SecurityOperation<TSecurityOperationCode> securityOperation)
            where TDomainObject : class, TPersistentDomainObjectBase;
}
