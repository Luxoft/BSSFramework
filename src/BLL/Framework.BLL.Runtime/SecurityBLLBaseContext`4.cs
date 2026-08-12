using Anch.HierarchicalExpand;
using Anch.SecuritySystem.AccessDenied;

using Framework.Application.Domain;
using Framework.Application.Events;
using Framework.BLL.Default;
using Framework.BLL.Services;

namespace Framework.BLL;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
    IServiceProvider serviceProvider,
    IEventOperationSender operationSender,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IRootSecurityService securityService,
    IAccessDeniedExceptionService accessDeniedExceptionService)
    : DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
      serviceProvider,
      operationSender,
      hierarchicalObjectExpanderFactory),
      ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
{
    public IRootSecurityService SecurityService { get; } = securityService;

    public IAccessDeniedExceptionService AccessDeniedExceptionService { get; } = accessDeniedExceptionService;
}
