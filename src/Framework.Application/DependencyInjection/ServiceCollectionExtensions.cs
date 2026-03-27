using CommonFramework.DependencyInjection;

using Framework.Application.Events;
using Framework.Application.FinancialYear;
using Framework.Application.Jobs;
using Framework.Application.Repository;
using Framework.Application.Repository.Default;
using Framework.Application.Repository.Generic;
using Framework.Database;
using Framework.Database.ExpressionVisitorContainer;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using SecuritySystem;

namespace Framework.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterListeners(Action<IDALListenerBuilder> setupAction) =>
            services.Initialize<DALListenerBuilder>(setupAction);

        public IServiceCollection AddNamedLocks(Action<IGenericNamedLockBuilder> setupAction) =>
            services.Initialize<GenericNamedLockBuilder>(setupAction);

        public IServiceCollection AddGenericApplicationServices()
        {
            services.TryAddSingleton(TimeProvider.System);

            services.RegisterFinancialYearServices();
            services.RegisterRepository();
            services.RegisterEvaluators();
            services.RegisterJobs();

            services.AddSingleton<IDBSessionEvaluator, DbSessionEvaluator>();

            services.AddScoped<IEventOperationSender, EventOperationSender>();

            return services;
        }

        private IServiceCollection RegisterRepository()
        {
            services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.Disabled), typeof(Repository<>));
            services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.View), typeof(ViewRepository<>));
            services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.Edit), typeof(EditRepository<>));

            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            services.AddScoped(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));
            services.AddScoped(typeof(IGenericRepositoryFactory<,>), typeof(GenericRepositoryFactory<,>));

            return services;
        }

        private IServiceCollection RegisterFinancialYearServices()
        {
            services.AddSingleton<IFinancialYearCalculator, FinancialYearCalculator>();
            services.AddSingleton<FinancialYearServiceSettings>();
            services.AddSingleton<IFinancialYearService, FinancialYearService>();

            return services;
        }

        private IServiceCollection RegisterEvaluators()
        {
            services.AddSingleton(typeof(IServiceEvaluator<>), typeof(ServiceEvaluator<>));

            return services;
        }

        private IServiceCollection RegisterJobs()
        {
            services.AddSingleton<IJobServiceEvaluatorFactory, JobServiceEvaluatorFactory>();
            services.AddSingleton(typeof(IJobServiceEvaluator<>), typeof(JobServiceEvaluator<>));
            services.AddScoped<IJobMiddlewareFactory, JobMiddlewareFactory>();

            return services;
        }
    }
}
