using Framework.Authorization.BLL;
using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.Events;
using Framework.OData.QueryLanguage;
using Framework.Tracking;

using GenericQueryable.Fetching;

using SecuritySystem.SecurityAccessor;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

using HierarchicalExpand;

using SecuritySystem.AccessDenied;
using SecuritySystem.UserSource;
using Framework.Configuration.BLL;

namespace SampleSystem.BLL;

public partial class SampleSystemBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices("BLL")] IEventOperationSender operationSender,
    ITrackingService<PersistentDomainObjectBase> trackingService,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IStandardExpressionBuilder standardExpressionBuilder,
    ISampleSystemValidator validator,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IRootSecurityService securityService,
    ISampleSystemBLLFactoryContainer logics,
    IAuthorizationBLLContext authorization,
    IConfigurationBLLContext configuration,
    BLLContextSettings<PersistentDomainObjectBase> settings,
    ISecurityAccessorResolver securityAccessorResolver,
    ICurrentUserSource<Employee> currentEmployeeSource,
    [FromKeyedServices(RootFetchRuleExpander.Key)] IFetchRuleExpander fetchRuleExpander)
    : SecurityBLLBaseContext<PersistentDomainObjectBase, Guid,
        ISampleSystemBLLFactoryContainer>(
        serviceProvider,
        operationSender,
        trackingService,
        accessDeniedExceptionService,
        standardExpressionBuilder,
        validator,
        hierarchicalObjectExpanderFactory)
{
    public IRootSecurityService SecurityService { get; } = securityService;

    public ISecurityAccessorResolver SecurityAccessorResolver { get; } = securityAccessorResolver;

    public override ISampleSystemBLLFactoryContainer Logics { get; } = logics;

    public IAuthorizationBLLContext Authorization { get; } = authorization;

    public IFetchRuleExpander FetchRuleExpander { get; } = fetchRuleExpander;

    public ICurrentUserSource<Employee> CurrentEmployeeSource { get; } = currentEmployeeSource;

    public IConfigurationBLLContext Configuration { get; } = configuration;

    public ITypeResolver<string> TypeResolver { get; } = settings.TypeResolver;
}
