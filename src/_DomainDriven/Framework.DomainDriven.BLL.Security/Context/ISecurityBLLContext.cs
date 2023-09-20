using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLContext<in TPersistentDomainObjectBase, TIdent> :

    IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDisabledSecurityProviderSource DisabledSecurityProviderSource { get; }

    ISecurityOperationResolver SecurityOperationResolver { get; }
}


public interface ISecurityBLLContext<out TAuthorizationBLLContext, in TPersistentDomainObjectBase, TIdent> : ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>,

    IAuthorizationBLLContextContainer<TAuthorizationBLLContext>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
}
