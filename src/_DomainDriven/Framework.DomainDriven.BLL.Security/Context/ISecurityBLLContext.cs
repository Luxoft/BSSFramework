using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLContext<in TPersistentDomainObjectBase, TIdent> :

    IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDisabledSecurityProviderSource DisabledSecurityProviderSource => this.ServiceProvider.GetRequiredService<IDisabledSecurityProviderSource>();

    ISecurityOperationResolver<TPersistentDomainObjectBase> SecurityOperationResolver => this.ServiceProvider.GetRequiredService<ISecurityOperationResolver<TPersistentDomainObjectBase>>();
}


public interface ISecurityBLLContext<out TAuthorizationBLLContext, in TPersistentDomainObjectBase, TIdent> : ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>,

    IAuthorizationBLLContextContainer<TAuthorizationBLLContext>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TAuthorizationBLLContext : IAuthorizationBLLContext<TIdent>
{
}
