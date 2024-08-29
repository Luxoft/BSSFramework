using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;

using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.SecuritySystem;

using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Authorization.SecuritySystem.Validation;
using Framework.Events;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.BLL;

public partial class AuthorizationBLLContext
{
    private readonly IAuthorizationBLLFactoryContainer logics;

    private readonly IDictionaryCache<string, SecurityContextType> securityContextTypeByNameCache;

    private readonly IDictionaryCache<Guid, SecurityContextType> securityContextTypeByIdCache;

    public AuthorizationBLLContext(
            IServiceProvider serviceProvider,
            [FromKeyedServices("BLL")] IEventOperationSender operationSender,
            ITrackingService<PersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            IAuthorizationValidator validator,
            IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            TimeProvider timeProvider,
            IRootSecurityService<PersistentDomainObjectBase> securityService,
            IAuthorizationBLLFactoryContainer logics,
            IAuthorizationExternalSource externalSource,
            INotificationPrincipalExtractor notificationPrincipalExtractor,
            ISecuritySystem securitySystem,
            IRunAsManager runAsManager,
            IAvailablePermissionSource availablePermissionSource,
            IAvailableSecurityRoleSource availableSecurityRoleSource,
            ICurrentPrincipalSource currentPrincipalSource,
            IPrincipalGeneralValidator principalValidator,
            ICurrentUser currentUser,
            BLLContextSettings<PersistentDomainObjectBase> settings,
            IAvailableSecurityOperationSource availableSecurityOperationSource)
            : base(
                   serviceProvider,
                   operationSender,
                   trackingService,
                   accessDeniedExceptionService,
                   standartExpressionBuilder,
                   validator,
                   hierarchicalObjectExpanderFactory,
                   fetchService)
    {
        this.TimeProvider = timeProvider;
        this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        this.logics = logics ?? throw new ArgumentNullException(nameof(logics));
        this.AvailablePermissionSource = availablePermissionSource;
        this.AvailableSecurityRoleSource = availableSecurityRoleSource;
        this.CurrentPrincipalSource = currentPrincipalSource;
        this.PrincipalValidator = principalValidator;
        this.CurrentUser = currentUser;
        this.AvailableSecurityOperationSource = availableSecurityOperationSource;
        this.NotificationPrincipalExtractor = notificationPrincipalExtractor;
        this.SecuritySystem = securitySystem;
        this.RunAsManager = runAsManager;

        this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));

        this.securityContextTypeByNameCache = new DictionaryCache<string, SecurityContextType>(
                                                                             domainTypeName => this.Logics.SecurityContextType.GetByName(domainTypeName, true),
                                                                             StringComparer.CurrentCultureIgnoreCase)
                .WithLock();

        this.securityContextTypeByIdCache = new DictionaryCache<Guid, SecurityContextType>(
                                                                         domainTypeId => this.Logics.SecurityContextType.GetById(domainTypeId, true))
                .WithLock();

        this.TypeResolver = settings.TypeResolver;
    }

    public ITypeResolver<string> TypeResolver { get; }

    public INotificationPrincipalExtractor NotificationPrincipalExtractor { get; }

    public ISecuritySystem SecuritySystem { get; }

    public IPrincipalGeneralValidator PrincipalValidator { get; }

    public ICurrentPrincipalSource CurrentPrincipalSource { get; }

    public ICurrentUser CurrentUser { get; }

    public IRunAsManager RunAsManager { get; }

    public IAvailablePermissionSource AvailablePermissionSource { get; }

    public IAvailableSecurityRoleSource AvailableSecurityRoleSource { get; }

    public IAvailableSecurityOperationSource AvailableSecurityOperationSource { get; }

    public IRootSecurityService<PersistentDomainObjectBase> SecurityService { get; }

    public override IAuthorizationBLLFactoryContainer Logics => this.logics;

    public IAuthorizationExternalSource ExternalSource { get; }

    public TimeProvider TimeProvider { get; }


    public SecurityContextType GetSecurityContextType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this.GetSecurityContextType(type.Name);
    }

    public SecurityContextType GetSecurityContextType(string domainTypeName)
    {
        if (domainTypeName == null) throw new ArgumentNullException(nameof(domainTypeName));

        return this.securityContextTypeByNameCache[domainTypeName];
    }

    public SecurityContextType GetSecurityContextType(Guid domainTypeId)
    {
        if (domainTypeId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainTypeId));

        return this.securityContextTypeByIdCache[domainTypeId];
    }

    IAuthorizationBLLContext IAuthorizationBLLContextContainer<IAuthorizationBLLContext>.Authorization => this;
}
