using Framework.Application.Domain;
using Framework.BLL.AccessDeniedExceptionService;
using Framework.BLL.BLL.Default;
using Framework.BLL.Context;
using Framework.Events;
using Framework.QueryLanguage;
using Framework.Validation;

using HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.AccessDenied;
using SecuritySystem.Providers;

namespace Framework.BLL;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
    IServiceProvider serviceProvider,
    IEventOperationSender operationSender,
    ITrackingService<TPersistentDomainObjectBase> trackingService,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IStandardExpressionBuilder standardExpressionBuilder,
    IValidator validator,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory)
    :
        DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
        serviceProvider,
        operationSender,
        trackingService,
        standardExpressionBuilder,
        validator,
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
