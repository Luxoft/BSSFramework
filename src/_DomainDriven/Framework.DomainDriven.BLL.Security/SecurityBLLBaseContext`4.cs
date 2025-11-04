using Framework.DomainDriven.Tracking;
using Framework.Events;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.Validation;

using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.HierarchicalExpand;
using SecuritySystem.Providers;

namespace Framework.DomainDriven.BLL.Security;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
    IServiceProvider serviceProvider,
    IEventOperationSender operationSender,
    ITrackingService<TPersistentDomainObjectBase> trackingService,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IStandartExpressionBuilder standartExpressionBuilder,
    IValidator validator,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
    :
        DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>(
        serviceProvider,
        operationSender,
        trackingService,
        standartExpressionBuilder,
        validator,
        hierarchicalObjectExpanderFactory,
        fetchService),
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
