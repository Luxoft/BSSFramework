using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.Events;
using Framework.QueryLanguage;

using SecuritySystem.SecurityAccessor;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

using HierarchicalExpand;

using SecuritySystem.AccessDenied;
using SecuritySystem.UserSource;

namespace SampleSystem.BLL;

public partial class SampleSystemBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices("BLL")] IEventOperationSender operationSender,
    ITrackingService<PersistentDomainObjectBase> trackingService,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IStandartExpressionBuilder standartExpressionBuilder,
    ISampleSystemValidator validator,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IRootSecurityService<PersistentDomainObjectBase> securityService,
    ISampleSystemBLLFactoryContainer logics,
    IAuthorizationBLLContext authorization,
    Framework.Configuration.BLL.IConfigurationBLLContext configuration,
    BLLContextSettings<PersistentDomainObjectBase> settings,
    ISecurityAccessorResolver securityAccessorResolver,
    ICurrentUserSource<Employee> currentEmployeeSource)
    : SecurityBLLBaseContext<PersistentDomainObjectBase, Guid,
        ISampleSystemBLLFactoryContainer>(
        serviceProvider,
        operationSender,
        trackingService,
        accessDeniedExceptionService,
        standartExpressionBuilder,
        validator,
        hierarchicalObjectExpanderFactory)
{
    public IRootSecurityService<PersistentDomainObjectBase> SecurityService { get; } = securityService;

    public ISecurityAccessorResolver SecurityAccessorResolver { get; } = securityAccessorResolver;

    public override ISampleSystemBLLFactoryContainer Logics { get; } = logics;

    public IAuthorizationBLLContext Authorization { get; } = authorization;

    public ICurrentUserSource<Employee> CurrentEmployeeSource { get; } = currentEmployeeSource;

    public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; } = configuration;

    public ITypeResolver<string> TypeResolver { get; } = settings.TypeResolver;

    public override bool AllowVirtualPropertyInOdata(Type domainType)
    {
        return base.AllowVirtualPropertyInOdata(domainType) || domainType == typeof(BusinessUnitProgramClass);
    }
}
