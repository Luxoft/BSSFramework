using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public abstract class DefaultSecurityBLLFactory<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TSecurityOperationCode, TIdent> : BLLContextContainer<TBLLContext>,

    IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, TIdent>

        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
        where TSecurityOperationCode : struct, Enum
{
    protected DefaultSecurityBLLFactory(TBLLContext context)
            : base(context)
    {
    }

    public virtual IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        throw new NotImplementedException();
    }

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        throw new NotImplementedException();
    }

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        throw new NotImplementedException();
    }

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(TSecurityOperationCode securityOperationCode)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        throw new NotImplementedException();
    }

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(SecurityOperation<TSecurityOperationCode> securityOperation)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        throw new NotImplementedException();
    }
}
