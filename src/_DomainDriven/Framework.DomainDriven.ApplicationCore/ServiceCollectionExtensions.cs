using CommonFramework.DependencyInjection;

using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.ApplicationCore.DALListeners;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.Repository;
using Framework.Events;
using Framework.Exceptions;
using Framework.FinancialYear;

using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.DomainDriven.ApplicationCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        services.AddSingleton<IExceptionExpander, ExceptionExpander>();

        services.AddScoped(typeof(IUpdateDeepLevelService<>), typeof(UpdateDeepLevelService<>));

        services.RegisterFinancialYearServices();
        services.RegisterRepository();
        services.RegisterAuthenticationServices();
        services.RegisterEvaluators();
        services.RegistryGenericDatabaseVisitors();

        services.AddSingleton<IInitializeManager, InitializeManager>();
        services.AddScoped<IEventOperationSender, EventOperationSender>();

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
        services.AddScopedFrom<IImpersonateService, ApplicationUserAuthenticationService>();

        return services;
    }

    private static IServiceCollection RegistryGenericDatabaseVisitors(this IServiceCollection services)
    {
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPersistentItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPeriodItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDefaultItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerMathItem>();

        services.AddSingleton<IIdPropertyResolver, IdPropertyResolver>();

        services.AddScoped<IExpressionVisitorContainer, ExpressionVisitorAggregator>();

        return services;
    }
}
