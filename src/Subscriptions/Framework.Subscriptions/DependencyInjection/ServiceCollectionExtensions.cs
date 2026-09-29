using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;

using Anch.Core;

using Framework.Infrastructure.DependencyInjection;
using Framework.Subscriptions.Metadata;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension<TSelf>(IBssFrameworkSetup<TSelf> setup)
        where TSelf : IBssFrameworkSetup<TSelf>
    {
        public TSelf AddSubscriptions<TEmployee>(
            Expression<Func<TEmployee, string>> emailPath,
            ImmutableArray<Assembly> assemblies,
            MailAddress? defaultSender = null)
            where TEmployee : class =>
            setup.AddServices(sc => sc.AddSubscriptions(emailPath, assemblies, defaultSender));
    }


    extension(IServiceCollection services)
    {
        public void AddSubscriptions<TEmployee>(
            Expression<Func<TEmployee, string>> emailPath,
            ImmutableArray<Assembly> assemblies,
            MailAddress? defaultSender = null)
            where TEmployee : class
        {
            services.AddSingleton<ISubscriptionResolver, SubscriptionResolver>();

            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<ISyncSubscriptionService, SyncSubscriptionService>();

            services.AddSingleton(new EmployeeInfo<TEmployee>(emailPath.ToPropertyAccessors()));
            services.AddScoped<INotificationEmailExtractor, NotificationEmailExtractor<TEmployee>>();

            if (defaultSender is not null)
            {
                services.AddKeyedSingleton(nameof(Subscriptions), (_, __) => defaultSender);
            }

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
