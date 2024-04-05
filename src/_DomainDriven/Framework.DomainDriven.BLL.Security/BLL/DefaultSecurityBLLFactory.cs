using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public abstract class DefaultSecurityBLLFactory<TBLLContext, TPersistentDomainObjectBase, TIdent> : BLLContextContainer<TBLLContext>,

    IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TIdent>

    where TBLLContext : class, ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>, ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    protected DefaultSecurityBLLFactory(TBLLContext context)
        : base(context)
    {
    }

    public virtual IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.Create<TDomainObject>(SecurityRule.Disabled);
    }

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(
        SecurityRule securityMode)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityMode));
    }

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(
        SecurityOperation securityOperation)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityOperation));
    }

    public abstract IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(
        ISecurityProvider<TDomainObject> securityProvider)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
