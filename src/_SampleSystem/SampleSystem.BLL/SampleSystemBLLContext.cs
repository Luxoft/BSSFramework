using Framework.Application.Events;
using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.BLL.Services;
using Framework.Configuration.BLL;
using Framework.Core.TypeResolving;
using Framework.Tracking;
using Framework.Validation;

using GenericQueryable.Fetching;

using HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using OData;
using SampleSystem.Domain;

using SecuritySystem.AccessDenied;
using SecuritySystem.SecurityAccessor;
using SecuritySystem.UserSource;

namespace SampleSystem.BLL;

public partial class SampleSystemBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices("BLL")] IEventOperationSender operationSender,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    ITrackingService<PersistentDomainObjectBase> trackingService,
    ISelectOperationParser selectOperationParser,
    ISampleSystemValidator validator,
    IRootSecurityService securityService,
    ISampleSystemBLLFactoryContainer logics,
    IAuthorizationBLLContext authorization,
    IConfigurationBLLContext configuration,
    BLLContextSettings<PersistentDomainObjectBase> settings,
    ISecurityAccessorResolver securityAccessorResolver,
    ICurrentUserSource<Employee> currentEmployeeSource,
    IFetchRuleExpander fetchRuleExpander)
    : SecurityBLLBaseContext<PersistentDomainObjectBase, Guid, ISampleSystemBLLFactoryContainer>(
        serviceProvider,
        operationSender,
        accessDeniedExceptionService,
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

    public ITrackingService<PersistentDomainObjectBase> TrackingService { get; } = trackingService;

    public ISelectOperationParser SelectOperationParser { get; } = selectOperationParser;

    public IValidator Validator { get; } = validator;
}
