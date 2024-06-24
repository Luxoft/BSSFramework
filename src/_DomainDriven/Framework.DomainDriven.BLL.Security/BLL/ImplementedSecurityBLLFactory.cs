using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class ImplementedSecurityBLLFactory<TBLLContext, TPersistentDomainObjectBase, TIdent> : DefaultSecurityBLLFactory<TBLLContext,
    TPersistentDomainObjectBase, TIdent>
    where TBLLContext : class, ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>,
    ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public ImplementedSecurityBLLFactory(TBLLContext context)
        : base(context)
    {
    }

    public sealed override IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(
        ISecurityProvider<TDomainObject> securityProvider)
    {
        var factory = this.Context.ServiceProvider.GetService<ISecurityBLLFactory<IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>, TDomainObject>>();

        if (factory == null)
        {
            return this.CreateDefault(securityProvider);
        }
        else
        {
            return factory.Create(securityProvider);
        }
    }

    protected abstract IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateDefault<TDomainObject>(
        ISecurityProvider<TDomainObject> securityProvider)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
