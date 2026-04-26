using Framework.Application.Events;
using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.BLL.Services;
using Framework.Configuration.BLL;
using Framework.Tracking;
using Framework.Validation;

using Anch.HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using Anch.OData;
using SampleSystem.Domain;
using SampleSystem.Domain.Employee;

using Anch.SecuritySystem.AccessDenied;
using Anch.SecuritySystem.SecurityAccessor;
using Anch.SecuritySystem.UserSource;

namespace SampleSystem.BLL;

public partial class SampleSystemBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices(nameof(BLL))] IEventOperationSender operationSender,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    ITrackingService<PersistentDomainObjectBase> trackingService,
    ISelectOperationParser selectOperationParser,
    ISampleSystemValidator validator,
    IRootSecurityService securityService,
    ISampleSystemBLLFactoryContainer logics,
    IAuthorizationBLLContext authorization,
    IConfigurationBLLContext configuration,
    ISecurityAccessorResolver securityAccessorResolver,
    ICurrentUserSource<Employee> currentEmployeeSource)
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

    public ICurrentUserSource<Employee> CurrentEmployeeSource { get; } = currentEmployeeSource;

    public IConfigurationBLLContext Configuration { get; } = configuration;

    public ITrackingService<PersistentDomainObjectBase> TrackingService { get; } = trackingService;

    public ISelectOperationParser SelectOperationParser { get; } = selectOperationParser;

    public IValidator Validator { get; } = validator;
}
