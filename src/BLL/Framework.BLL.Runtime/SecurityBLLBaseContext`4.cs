using Framework.Application.Domain;
using Framework.Application.Events;
using Framework.BLL.Default;

using Anch.HierarchicalExpand;

using Anch.SecuritySystem.AccessDenied;

namespace Framework.BLL;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
    IServiceProvider serviceProvider,
    IEventOperationSender operationSender,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory)
    : DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
      serviceProvider,
      operationSender,
      hierarchicalObjectExpanderFactory),
      IAccessDeniedExceptionServiceContainer,
      ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
{
    public IAccessDeniedExceptionService AccessDeniedExceptionService { get; } = accessDeniedExceptionService;
}
