using Anch.DependencyInjection;

using Framework.Application.Events;
using Framework.Application.FinancialYear;
using Framework.Application.Jobs;
using Framework.Application.Repository;
using Framework.Application.Repository.Default;
using Framework.Application.Repository.Generic;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Anch.SecuritySystem;

namespace Framework.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddListeners(Action<IDALListenerSetup> setupAction) =>
            services.Initialize<DALListenerSetup>(setupAction);

        public IServiceCollection AddNamedLocks(Action<IGenericNamedLockSetup> setupAction) =>
            services.Initialize<GenericNamedLockSetup>(setupAction);

        public IServiceCollection AddGenericApplicationServices()
        {
            services.TryAddSingleton(TimeProvider.System);

            services.AddFinancialYearServices();
            services.AddRepository();
            services.AddEvaluators();
            services.AddJobs();

            services.AddSingleton<IDBSessionEvaluator, DbSessionEvaluator>();

            services.AddScoped<IEventOperationSender, EventOperationSender>();

            return services;
        }

        private IServiceCollection AddRepository()
        {
            services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.Disabled), typeof(Repository<>));
            services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.View), typeof(ViewRepository<>));
            services.AddKeyedScoped(typeof(IRepository<>), nameof(SecurityRule.Edit), typeof(EditRepository<>));

            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            services.AddScoped(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));
            services.AddScoped(typeof(IGenericRepositoryFactory<,>), typeof(GenericRepositoryFactory<,>));

            return services;
        }

        private IServiceCollection AddFinancialYearServices()
        {
            services.AddSingleton<IFinancialYearCalculator, FinancialYearCalculator>();
            services.AddSingleton<FinancialYearServiceSettings>();
            services.AddSingleton<IFinancialYearService, FinancialYearService>();

            return services;
        }

        private IServiceCollection AddEvaluators()
        {
            services.AddSingleton(typeof(IServiceEvaluator<>), typeof(ServiceEvaluator<>));

            return services;
        }

        private IServiceCollection AddJobs()
        {
            services.AddSingleton<IJobServiceEvaluatorFactory, JobServiceEvaluatorFactory>();
            services.AddSingleton(typeof(IJobServiceEvaluator<>), typeof(JobServiceEvaluator<>));
            services.AddScoped<IJobMiddlewareFactory, JobMiddlewareFactory>();

            return services;
        }
    }
}
