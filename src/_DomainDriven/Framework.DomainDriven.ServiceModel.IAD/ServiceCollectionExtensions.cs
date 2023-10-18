using Framework.Authorization;
using Framework.Authorization.Domain;
using Framework.Authorization.Environment;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.DomainServices;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Authorization.SecuritySystem.OperationInitializer;
using Framework.Configuration;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.Repository;
using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Bss;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); //TODO: add unsecurity di key "DisabledSecurity" after update to NET8.0
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));


        services.AddScoped(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));
        services.AddScoped(typeof(IGenericRepositoryFactory<,>), typeof(GenericRepositoryFactory<,>));

        services.AddSingleton<IDBSessionEvaluator, DBSessionEvaluator>();
        services.AddSingleton(typeof(IServiceEvaluator<>), typeof(ServiceEvaluator<>));

        services.RegisterAuthorizationSystem();

        services.AddSingleton(new SecurityOperationTypeInfo(typeof(BssSecurityOperation)));
        services.RegisterAuthorizationSecurity();
        services.RegisterConfigurationSecurity();

        services.AddSingleton<IDateTimeService>(DateTimeService.Default);

        services.AddScoped(typeof(INotImplementedDomainSecurityService<>), typeof(OnlyDisabledDomainSecurityService<>));

        return services;
    }

    public static IServiceCollection RegistryGenericDatabaseVisitors(this IServiceCollection services)
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

    public static IServiceCollection RegisterHierarchicalObjectExpander<TPersistentDomainObjectBase>(this IServiceCollection services)
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        return services.AddSingleton<IRealTypeResolver, IdentityRealTypeResolver>()
                       .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<Guid>>();
    }

    private static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
    {
        return services.AddScoped<IAuthorizationSystem<Guid>, AuthorizationSystem>()
                       .AddScopedFrom<IAuthorizationSystem, IAuthorizationSystem<Guid>>()
                       .AddScopedFrom<IOperationAccessor, IAuthorizationSystem>()

                       .AddSingleton<IAccessDeniedExceptionService, AccessDeniedExceptionService<Guid>>()

                       .AddSingleton<IDisabledSecurityProviderSource, DisabledSecurityProviderSource>()

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

                       .AddScoped<IAuthorizationOperationInitializer, AuthorizationOperationInitializer>()

                       .AddSingleton<ISecurityContextInfoService, SecurityContextInfoService>()

                       .AddScoped<IAuthorizationExternalSource, AuthorizationExternalSource>();
    }


    public static IServiceCollection RegisterAuthorizationSecurity(this IServiceCollection services)
    {
        return services.AddSingleton(new SecurityOperationTypeInfo(typeof(AuthorizationSecurityOperation)))

                       .RegisterDomainSecurityServices<Guid>(
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
        return services.AddSingleton(new SecurityOperationTypeInfo(typeof(ConfigurationSecurityOperation)))

                       .RegisterDomainSecurityServices<Guid>(
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
