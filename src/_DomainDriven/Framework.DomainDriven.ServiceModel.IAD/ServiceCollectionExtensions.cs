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
using Framework.FinancialYear;
using Framework.HierarchicalExpand;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Bss;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBssFramework(this IServiceCollection services, Action<IBssFrameworkSettings> setupAction)
    {
        var settings = new BssFrameworkSettings();

        setupAction?.Invoke(settings);
        settings.InitSettings();

        foreach (var securityOperationType in settings.SecurityOperationTypes)
        {
            services.AddSingleton(new SecurityOperationTypeInfo(securityOperationType));
        }

        foreach (var namedLockType in settings.NamedLockTypes)
        {
            services.AddSingleton(new NamedLockTypeInfo(namedLockType));
        }

        services.TryAddSingleton(TimeProvider.System);

        services.RegisterFinancialYearServices();
        services.RegisterRepository();
        services.RegisterAuthenticationServices();
        services.RegisterEvaluators();
        services.RegisterAuthorizationSystem(); ;
        services.RegisterAuthorizationSecurity();
        services.RegisterConfigurationSecurity();
        services.RegisterNamedLocks();
        services.RegisterHierarchicalObjectExpander();
        services.RegistryGenericDatabaseVisitors();

        services.AddSingleton<IInitializeManager, InitializeManager>();

        return services;
    }
    private static void InitSettings(this BssFrameworkSettings settings)
    {
        if (settings.RegisterBaseSecurityOperationTypes)
        {
            settings.SecurityOperationTypes.Add(typeof(BssSecurityOperation));
            settings.SecurityOperationTypes.Add(typeof(AuthorizationSecurityOperation));
            settings.SecurityOperationTypes.Add(typeof(ConfigurationSecurityOperation));
        }

        if (settings.RegisterBaseNamedLockTypes)
        {
            settings.NamedLockTypes.Add(typeof(ConfigurationNamedLock));
        }
    }

    private static IServiceCollection RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddKeyedScoped(typeof(IRepository<>), BLLSecurityMode.Disabled, typeof(Repository<>));
        services.AddKeyedScoped(typeof(IRepository<>), BLLSecurityMode.View, typeof(ViewRepository<>));
        services.AddKeyedScoped(typeof(IRepository<>), BLLSecurityMode.Edit, typeof(EditRepository<>));

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

                       .AddSingleton<ISecurityOperationResolver, SecurityOperationResolver>()

                       .AddSingleton<ISecurityOperationParser<Guid>, SecurityOperationParser<Guid>>()
                       .AddSingletonFrom<ISecurityOperationParser, ISecurityOperationParser<Guid>>()

                       .AddScoped<IOperationDomainService, OperationDomainService>()
                       .AddScoped<IBusinessRoleDomainService, BusinessRoleDomainService>()

                       .AddScoped<IAvailableSecurityOperationSource, AvailableSecurityOperationSource>()

                       .AddSingleton<ISecurityRoleSource, SecurityRoleSource>()

                       .AddSingleton<InitializeSettings>()
                       .AddScoped<IAuthorizationEntityTypeInitializer, AuthorizationEntityTypeInitializer>()
                       .AddScoped<IAuthorizationOperationInitializer, AuthorizationOperationInitializer>()
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

                                   .Add<Operation>(
                                       b => b.SetView(AuthorizationSecurityOperation.OperationView)
                                             .SetEdit(AuthorizationSecurityOperation.OperationEdit)
                                             .SetCustomService<AuthorizationOperationSecurityService>())

                                   .Add<EntityType>(
                                       b => b.SetView(AuthorizationSecurityOperation.Disabled)));
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
                                       b => b.SetView(ConfigurationSecurityOperation.Disabled)));
    }
}
