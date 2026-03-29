using Framework.Database._Visitors.Specific;
using Framework.Database.ExpressionVisitorContainer;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddGenericDatabaseServices()
        {
            services.AddSingleton<IInitializeManager, InitializeManager>();

            services.AddScoped<IDBSessionManager, DBSessionManager>();

            services.RegistryGenericDatabaseVisitors();
            services.AddScoped<IExpressionVisitorContainer, ExpressionVisitorAggregator>();

            return services;
        }
        private IServiceCollection RegistryGenericDatabaseVisitors()
        {
            //services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPersistentItem>();
            services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPeriodItem>();
            services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDefaultItem>();
            services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerMathItem>();

            services.AddSingleton<IIdPropertyResolver, IdPropertyResolver>();

            return services;
        }
    }
}
