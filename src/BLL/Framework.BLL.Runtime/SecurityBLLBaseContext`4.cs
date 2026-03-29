using Framework.Application.Domain;
using Framework.Application.Events;
using Framework.BLL.Default;
using Framework.OData.QueryLanguage.StandardExpressionBuilder;

using HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.AccessDenied;
using SecuritySystem.Providers;

namespace Framework.BLL;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
    IServiceProvider serviceProvider,
    IEventOperationSender operationSender,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IStandardExpressionBuilder standardExpressionBuilder,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory)
    : DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
      serviceProvider,
      operationSender,
      standardExpressionBuilder,
      hierarchicalObjectExpanderFactory),
      IAccessDeniedExceptionServiceContainer,
      ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
{
    public virtual IAccessDeniedExceptionService AccessDeniedExceptionService { get; } = accessDeniedExceptionService;

    public ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.ServiceProvider.GetRequiredService<ISecurityProvider<TDomainObject>>();
    }
}
