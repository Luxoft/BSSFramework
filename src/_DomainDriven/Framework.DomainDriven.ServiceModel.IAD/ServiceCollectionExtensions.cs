using Framework.Authorization.SecuritySystem;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.Repository;
using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddScoped(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));
        services.AddScoped(typeof(IGenericRepositoryFactory<,>), typeof(GenericRepositoryFactory<,>));

        services.AddSingleton<IDBSessionEvaluator, DBSessionEvaluator>();

        services.RegisterAuthorizationSystem();

        services.AddSingleton<IDateTimeService>(DateTimeService.Default);

        services.AddScoped(typeof(INotImplementedDomainSecurityService<>), typeof(OnlyDisabledDomainSecurityService<>));

        return services;
    }

    public static IServiceCollection RegistryGenericDatabaseVisitors(this IServiceCollection services)
    {
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>();

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
        return services.AddScopedFromLazyInterfaceImplement<IAuthorizationSystem<Guid>, AuthorizationSystem>() //TODO: Temp hack
                       .AddScopedFrom<IAuthorizationSystem, IAuthorizationSystem<Guid>>()
                       .AddScopedFrom<IOperationAccessor, IAuthorizationSystem>()

                       .AddSingleton<IDomainObjectIdentResolver, DomainObjectIdentResolver<Guid>>()
                       .AddSingleton<IAccessDeniedExceptionService, AccessDeniedExceptionService>()

                       .AddScoped<ISecurityContextInfoService<Guid>, SecurityContextInfoService>()

                       .AddSingleton<IDisabledSecurityProviderSource, DisabledSecurityProviderSource>()

                       .AddScoped<IRunAsManager, RunAsManger>()
                       .AddScoped<IRuntimePermissionOptimizationService, RuntimePermissionOptimizationService>()

                       .AddScoped<IAvailablePermissionSource, AvailablePermissionSource>()
                       .AddScoped<ICurrentPrincipalSource, CurrentPrincipalSource>()

                       .AddScoped<IOperationAccessorFactory, OperationAccessorFactory>()

                       .AddScoped<IQueryableSource, RepositoryQueryableSource>()

                       .AddScoped<ISecurityExpressionBuilderFactory, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Guid>>()

                       .AddSingleton<ISecurityOperationResolver, SecurityOperationResolver>();
    }
}
