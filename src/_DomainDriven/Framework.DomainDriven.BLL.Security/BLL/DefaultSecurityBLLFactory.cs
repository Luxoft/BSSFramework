using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public abstract class DefaultSecurityBLLFactory<TBLLContext, TPersistentDomainObjectBase, TIdent> : BLLContextContainer<TBLLContext>,

    IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TIdent>

        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
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

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(SecurityOperation securityOperation)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        throw new NotImplementedException();
    }
}
