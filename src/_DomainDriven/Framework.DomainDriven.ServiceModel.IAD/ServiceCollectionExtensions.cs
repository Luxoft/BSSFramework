using Framework.Authorization;
using Framework.Authorization.Domain;
using Framework.Authorization.Environment;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.DomainServices;
using Framework.Authorization.SecuritySystem.ExternalSource;

using Framework.Authorization.SecuritySystem.Initialize;
using Framework.Authorization.SecuritySystem.PermissionOptimization;
using Framework.Configuration;
using Framework.Configuration.Domain;
using Framework.Configuration.NamedLocks;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven.ImpersonateService;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.Repository;
using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.Events;
using Framework.FinancialYear;
using Framework.HierarchicalExpand;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        services.RegisterFinancialYearServices();
        services.RegisterRepository();
        services.RegisterAuthenticationServices();
        services.RegisterEvaluators();
        services.RegisterAuthorizationSystem();
        services.RegisterAuthorizationSecurity();
        services.RegisterConfigurationSecurity();
        services.RegisterNamedLocks();
        services.RegisterHierarchicalObjectExpander();
        services.RegistryGenericDatabaseVisitors();

        services.AddSingleton<IInitializeManager, InitializeManager>();
        services.AddScoped<IEventOperationSender, EventOperationSender>();

        return services;
    }

    public static IServiceCollection RegisterListeners(this IServiceCollection services, Action<IListenerSetupObject> setup)
    {
        var setupObject = new ListenerSetupObject();

        setup(setupObject);

        foreach (var setupObjectInitAction in setupObject.InitActions)
        {
            setupObjectInitAction(services);
        }

        return services;
    }

    private static IServiceCollection RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.Disabled), typeof(Repository<>));
        services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.View), typeof(ViewRepository<>));
        services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.Edit), typeof(EditRepository<>));

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        services.AddScoped(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));
        services.AddScoped(typeof(IGenericRepositoryFactory<,>), typeof(GenericRepositoryFactory<,>));

        return services;
    }

    private static IServiceCollection RegisterFinancialYearServices(this IServiceCollection services)
    {
        services.AddSingleton<IFinancialYearCalculator, FinancialYearCalculator>();
        services.AddSingleton<FinancialYearServiceSettings>();
        services.AddSingleton<IFinancialYearService, FinancialYearService>();

        return services;
    }

    private static IServiceCollection RegisterEvaluators(this IServiceCollection services)
    {
        services.AddSingleton<IDBSessionEvaluator, DBSessionEvaluator>();
        services.AddSingleton(typeof(IServiceEvaluator<>), typeof(ServiceEvaluator<>));

        return services;
    }

    private static IServiceCollection RegisterAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ApplicationUserAuthenticationService>();
        services.AddScopedFrom<IUserAuthenticationService, ApplicationUserAuthenticationService>();
        services.AddScopedFrom<IImpersonateService, ApplicationUserAuthenticationService>();

        return services;
    }

    private static IServiceCollection RegistryGenericDatabaseVisitors(this IServiceCollection services)
    {
        services
            .AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<
                Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>();
        services
            .AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<
                Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>();

        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPersistentItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPeriodItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDefaultItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerMathItem>();

        services.AddSingleton<IIdPropertyResolver, IdPropertyResolver>();

        services.AddScoped<IExpressionVisitorContainer, ExpressionVisitorAggregator>();

        return services;
    }

    private static IServiceCollection RegisterHierarchicalObjectExpander(this IServiceCollection services)
    {
        return services.AddSingleton<IRealTypeResolver, IdentityRealTypeResolver>()
                       .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<Guid>>();
    }

    private static IServiceCollection RegisterNamedLocks(this IServiceCollection services)
    {
        return services
               .AddScoped<INamedLockService, NamedLockService>()
               .AddScoped<INamedLockInitializer, NamedLockInitializer>()
               .AddSingleton<INamedLockSource, NamedLockSource>();
    }

    private static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
    {
        return services.AddScoped<IAuthorizationSystem<Guid>, AuthorizationSystem>()
                       .AddScopedFrom<IAuthorizationSystem, IAuthorizationSystem<Guid>>()
                       .AddScopedFrom<IOperationAccessor, IAuthorizationSystem>()

                       .AddSingleton<IAccessDeniedExceptionService, AccessDeniedExceptionService<Guid>>()

                       .AddScoped(typeof(ISecurityProvider<>), typeof(DisabledSecurityProvider<>))
                       .AddScoped(typeof(IDomainSecurityService<>), typeof(OnlyDisabledDomainSecurityService<>))

                       .AddScoped<IRunAsManager, RunAsManger>()
                       .AddScoped<IRuntimePermissionOptimizationService, RuntimePermissionOptimizationService>()

                       .AddScoped<IActualPrincipalSource, ActualPrincipalSource>()
                       .AddScoped<IAvailablePermissionSource, AvailablePermissionSource>()
                       .AddScoped<ICurrentPrincipalSource, CurrentPrincipalSource>()

                       .AddScoped<IOperationAccessorFactory, OperationAccessorFactory>()

                       .AddScoped<IQueryableSource, RepositoryQueryableSource>()

                       .AddScoped<ISecurityExpressionBuilderFactory, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.
                           SecurityExpressionBuilderFactory<Guid>>()


                       .AddSingleton<SecurityModeExpander>()
                       .AddSingleton<SecurityOperationExpander>()
                       .AddSingleton<SecurityRoleExpander>()

                       .AddSingleton<ISecurityRuleExpander, SecurityRuleExpander>()

                       .AddSingleton<ISecurityRoleParser, SecurityRoleParser>()

                       .AddScoped<IBusinessRoleDomainService, BusinessRoleDomainService>()

                       .AddScoped<IAvailableSecurityRoleSource, AvailableSecurityRoleSource>()

                       .AddSingleton<ISecurityRoleSource, SecurityRoleSource>()

                       .AddSingleton<InitializeSettings>()
                       .AddScoped<IAuthorizationEntityTypeInitializer, AuthorizationEntityTypeInitializer>()
                       .AddScoped<IAuthorizationBusinessRoleInitializer, AuthorizationBusinessRoleInitializer>()

                       .AddSingleton<ISecurityContextInfoService, SecurityContextInfoService>()

                       .AddScoped<IAuthorizationExternalSource, AuthorizationExternalSource>();
    }


    public static IServiceCollection RegisterAuthorizationSecurity(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(
                           rb => rb.Add<Principal>(
                                       b => b.SetView(AuthorizationSecurityOperation.PrincipalView)
                                             .SetEdit(AuthorizationSecurityOperation.PrincipalEdit)
                                             .SetCustomService<AuthorizationPrincipalSecurityService>())

                                   .Add<Permission>(
                                       b => b.SetView(AuthorizationSecurityOperation.PrincipalView)
                                             .SetEdit(AuthorizationSecurityOperation.PrincipalEdit)
                                             .SetCustomService<AuthorizationPermissionSecurityService>())

                                   .Add<BusinessRole>(
                                       b => b.SetView(AuthorizationSecurityOperation.BusinessRoleView)
                                             .SetEdit(AuthorizationSecurityOperation.BusinessRoleEdit)
                                             .SetCustomService<AuthorizationBusinessRoleSecurityService>())

                                   .Add<EntityType>(
                                       b => b.SetView(SecurityRule.Disabled)));
    }

    public static IServiceCollection RegisterConfigurationSecurity(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(
                           rb => rb.Add<ExceptionMessage>(
                                       b => b.SetView(ConfigurationSecurityOperation.ExceptionMessageView))

                                   .Add<Sequence>(
                                       b => b.SetView(ConfigurationSecurityOperation.SequenceView)
                                             .SetEdit(ConfigurationSecurityOperation.SequenceEdit))

                                   .Add<SystemConstant>(
                                       b => b.SetView(ConfigurationSecurityOperation.SystemConstantView)
                                             .SetEdit(ConfigurationSecurityOperation.SystemConstantEdit))

                                   .Add<CodeFirstSubscription>(
                                       b => b.SetView(ConfigurationSecurityOperation.SubscriptionView)
                                             .SetEdit(ConfigurationSecurityOperation.SubscriptionEdit))

                                   .Add<TargetSystem>(
                                       b => b.SetView(ConfigurationSecurityOperation.TargetSystemView)
                                             .SetEdit(ConfigurationSecurityOperation.TargetSystemEdit))

                                   .Add<DomainType>(
                                       b => b.SetView(SecurityRule.Disabled)));
    }
}
