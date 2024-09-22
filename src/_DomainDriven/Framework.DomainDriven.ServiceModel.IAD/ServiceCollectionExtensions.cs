using Framework.Configuration.Domain;
using Framework.Configuration.NamedLocks;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.ApplicationCore.ExternalSource;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Jobs;
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
using Framework.SecuritySystem.PersistStorage;

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
        services.RegisterConfigurationSecurity();
        services.RegisterNamedLocks();
        services.RegisterHierarchicalObjectExpander();
        services.RegistryGenericDatabaseVisitors();

        services.AddSingleton<IInitializeManager, InitializeManager>();
        services.AddScoped<IEventOperationSender, EventOperationSender>();

        services.AddScoped<IQueryableSource, RepositoryQueryableSource>();
        services.AddScoped(typeof(IPersistStorage<>), typeof(PersistStorage<>));

        services.AddExternalSource();

        services.AddSingleton<SecurityAdministratorRuleFactory>();

        services.AddSingleton<IJobServiceEvaluatorFactory, JobServiceEvaluatorFactory>();
        services.AddSingleton(typeof(IJobServiceEvaluator<>), typeof(JobServiceEvaluator<>));
        services.AddScoped<IJobMiddlewareFactory, JobMiddlewareFactory>();

        return services;
    }

    public static IServiceCollection RegisterListeners(this IServiceCollection services, Action<IDALListenerSetupObject> setup)
    {
        var setupObject = new DALListenerSetupObject();

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
                PersistentDomainObjectBase, Guid>>();

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

    public static IServiceCollection RegisterConfigurationSecurity(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices(

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
