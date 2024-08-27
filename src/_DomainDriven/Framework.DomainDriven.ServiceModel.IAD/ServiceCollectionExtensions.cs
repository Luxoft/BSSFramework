using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.ExternalSource;

using Framework.Authorization.SecuritySystem.Initialize;
using Framework.Authorization.SecuritySystem.PermissionOptimization;
using Framework.Authorization.SecuritySystem.Validation;
using Framework.Configuration.Domain;
using Framework.Configuration.NamedLocks;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.Repository;
using Framework.Events;
using Framework.Exceptions;
using Framework.FinancialYear;
using Framework.HierarchicalExpand.DependencyInjection;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        services.AddSingleton<IExceptionExpander, ExceptionExpander>();

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

    private static IServiceCollection RegisterNamedLocks(this IServiceCollection services)
    {
        return services
               .AddScoped<INamedLockService, NamedLockService>()
               .AddScoped<INamedLockInitializer, NamedLockInitializer>()
               .AddSingleton<INamedLockSource, NamedLockSource>();
    }

    private static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
    {
        return services.AddScoped<IQueryableSource, RepositoryQueryableSource>()
                       .AddScoped<IAuthorizationSystem<Guid>, AuthorizationSystem>()

                       .AddSingleton<ISecurityRolesIdentsResolver, SecurityRolesIdentsResolver>()

                       .AddScoped<IRunAsManager, RunAsManger>()
                       .AddScoped<IRuntimePermissionOptimizationService, RuntimePermissionOptimizationService>()

                       .AddScoped<IAvailablePermissionSource, AvailablePermissionSource>()
                       .AddScoped<ICurrentPrincipalSource, CurrentPrincipalSource>()

                       .AddScoped<IAuthorizationSystemFactory, AuthorizationSystemFactory>()

                       .AddScoped<IAvailableSecurityRoleSource, AvailableSecurityRoleSource>()
                       .AddScoped<IAvailableSecurityOperationSource, AvailableSecurityOperationSource>()

                       .AddSingleton<InitializerSettings>()
                       .AddScoped<IAuthorizationSecurityContextInitializer, AuthorizationSecurityContextInitializer>()
                       .AddScoped<IAuthorizationBusinessRoleInitializer, AuthorizationBusinessRoleInitializer>()

                       .AddScoped<IAuthorizationExternalSource, AuthorizationExternalSource>()
                       .AddScoped(typeof(LocalStorage<>))

                       .AddScoped<IPrincipalDomainService, PrincipalDomainService>()

                       .AddScoped<IPrincipalGeneralValidator, PrincipalGeneralValidator>()
                       .AddKeyedScoped<IValidator<Principal>, PrincipalUniquePermissionValidator>(PrincipalUniquePermissionValidator.Key)
                       .AddKeyedScoped<IValidator<Permission>, PermissionGeneralValidator>(PermissionGeneralValidator.Key)
                       .AddKeyedScoped<IValidator<Permission>, PermissionDelegateValidator>(PermissionDelegateValidator.Key)
                       .AddKeyedScoped<IValidator<Permission>, PermissionRequiredContextValidator>(PermissionRequiredContextValidator.Key)
                       .AddScoped<IValidator<PermissionRestriction>, PermissionRestrictionValidator>()

                       .AddSingleton<ISecurityAccessorDataOptimizer, SecurityAccessorDataOptimizer>()
                       .AddScoped<ISecurityAccessorInfinityStorage, AuthorizationAccessorInfinityStorage>()
                       .AddScoped<ISecurityAccessorDataEvaluator, SecurityAccessorDataEvaluator>()
                       .AddScoped<ISecurityAccessorResolver, SecurityAccessorResolver>();
    }

    public static IServiceCollection RegisterAuthorizationSecurity(this IServiceCollection services)
    {
        var securityAdministratorRule = AuthorizationSecurityRule.SecurityAdministrator;

        var principalViewSecurityRule = securityAdministratorRule.Or(AuthorizationSecurityRule.CurrentPrincipal);

        return services

               .AddSingleton<SecurityAdministratorRuleFactory>()

               .AddRelativeDomainPath((Permission permission) => permission.Principal)
               .AddScoped(typeof(CurrentPrincipalSecurityProvider<>))

               .AddRelativeDomainPath((BusinessRole businessRole) => businessRole)
               .AddScoped(typeof(AvailableBusinessRoleSecurityProvider<>))

               .AddRelativeDomainPath((Permission permission) => permission.DelegatedFrom, nameof(Permission.DelegatedFrom))
               .AddScoped(typeof(DelegatedFromSecurityProvider<>))

               .RegisterDomainSecurityServices<Guid>(
                   rb => rb
                         .Add<Principal>(
                             b => b.SetView(principalViewSecurityRule)
                                   .SetEdit(securityAdministratorRule))

                         .Add<Permission>(
                             b => b.SetView(principalViewSecurityRule.Or(AuthorizationSecurityRule.DelegatedFrom))
                                   .SetEdit(securityAdministratorRule.Or(AuthorizationSecurityRule.DelegatedFrom)))

                         .Add<BusinessRole>(
                             b => b.SetView(securityAdministratorRule.Or(AuthorizationSecurityRule.AvailableBusinessRole))
                                   .SetEdit(securityAdministratorRule))

                         .Add<SecurityContextType>(
                             b => b.SetView(SecurityRule.Disabled)));
    }

    public static IServiceCollection RegisterConfigurationSecurity(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(

                           rb => rb.Add<ExceptionMessage>(
                                       b => b.SetView(SecurityRole.Administrator))

                                   .Add<Sequence>(
                                       b => b.SetView(SecurityRole.Administrator)
                                             .SetEdit(SecurityRole.Administrator))

                                   .Add<SystemConstant>(
                                       b => b.SetView(SecurityRole.Administrator)
                                             .SetEdit(SecurityRole.Administrator))

                                   .Add<CodeFirstSubscription>(
                                       b => b.SetView(SecurityRole.Administrator)
                                             .SetEdit(SecurityRole.Administrator))

                                   .Add<TargetSystem>(
                                       b => b.SetView(SecurityRole.Administrator)
                                             .SetEdit(SecurityRole.Administrator))

                                   .Add<DomainType>(
                                       b => b.SetView(SecurityRule.Disabled)));
    }
}
