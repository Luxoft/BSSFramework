using Framework.Application.Domain;
using Framework.BLL.Context;
using Framework.BLL.Services;

using SecuritySystem;
using SecuritySystem.Providers;

namespace Framework.BLL.BLL;

public abstract class DefaultSecurityBLLFactory<TBLLContext, TPersistentDomainObjectBase, TIdent>(TBLLContext context) : BLLContextContainer<TBLLContext>(context),
    IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TIdent>
    where TBLLContext : class, ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>, ISecurityServiceContainer<IRootSecurityService>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public virtual IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.Create<TDomainObject>(SecurityRule.Disabled);
    }

    public virtual IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(
        SecurityRule securityRule)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityRule));
    }

    public abstract IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(
        ISecurityProvider<TDomainObject> securityProvider)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
