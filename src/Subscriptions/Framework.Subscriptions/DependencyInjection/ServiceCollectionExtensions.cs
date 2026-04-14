using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Infrastructure.DependencyInjection;
using Framework.Subscriptions.Metadata;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension<TSelf>(IBssFrameworkSetup<TSelf> setup)
        where TSelf : IBssFrameworkSetup<TSelf>
    {
        public TSelf AddSubscriptions<TEmployee, TPrincipal>(
            Expression<Func<TEmployee, string>> emailPath,
            ImmutableArray<Assembly> assemblies)
            where TEmployee : class =>
            setup.AddServices(sc => sc.AddSubscriptions<TEmployee, TPrincipal>(emailPath, assemblies));
    }


    extension(IServiceCollection services)
    {
        public void AddSubscriptions<TEmployee, TPrincipal>(
            Expression<Func<TEmployee, string>> emailPath,
            ImmutableArray<Assembly> assemblies)
            where TEmployee : class
        {
            services.AddSingleton<ISubscriptionResolver, SubscriptionResolver>();

            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddSingleton(new EmployeeInfo<TEmployee>(emailPath.ToPropertyAccessors()));
            services.AddScoped<IEmployeeEmailExtractor, EmployeeEmailExtractor<TEmployee, TPrincipal>>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsAbstract && typeof(ISubscription).IsAssignableFrom(type))
                    {
                        services.AddSingleton(typeof(ISubscription), type);
                    }
                }
            }
        }
    }
}
