using Framework.Application.Domain;
using Framework.BLL.Services;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Providers;

namespace Framework.BLL;

public abstract class ImplementedSecurityBLLFactory<TBLLContext, TPersistentDomainObjectBase, TIdent>(TBLLContext context) :
    DefaultSecurityBLLFactory<TBLLContext, TPersistentDomainObjectBase, TIdent>(context)
    where TBLLContext : class, ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>, ISecurityServiceContainer<IRootSecurityService>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public sealed override IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>(
        ISecurityProvider<TDomainObject> securityProvider)
    {
        var factory =
            this.Context.ServiceProvider.GetService<ISecurityBLLFactory<IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>, TDomainObject>>();

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
