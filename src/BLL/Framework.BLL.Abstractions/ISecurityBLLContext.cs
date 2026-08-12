using Anch.SecuritySystem.AccessDenied;

using Framework.Application.Domain;
using Framework.BLL.Services;

namespace Framework.BLL;

public interface ISecurityBLLContext : IDefaultBLLContext
{
    IRootSecurityService SecurityService { get; }

    IAccessDeniedExceptionService AccessDeniedExceptionService { get; }
}


public interface ISecurityBLLContext<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>, ISecurityBLLContext
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;
