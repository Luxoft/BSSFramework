using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Subscriptions;
using Framework.Notification;
using Framework.Notification.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class BssFrameworkSettingsExtensions
{
    extension(IBssFrameworkBuilder builder)
    {
        public IBssFrameworkBuilder AddSystemConstant(Type systemConstantContainerType) => builder.AddServices(services => services.AddSingleton(new SystemConstantInfo(systemConstantContainerType)));

        public IBssFrameworkBuilder AddLegacyDefaultGenericServices() => builder.AddServices(services => services.AddLegacyDefaultGenericServices());

        public IBssFrameworkBuilder AddConfigurationTargetSystems(Action<ITargetSystemRootSettings> setupAction) =>
            builder.AddServices(
                services =>
                {
                    var tsSettings = new TargetSystemRootSettings();

                    setupAction.Invoke(tsSettings);

                    if (tsSettings.RegisterBase)
                    {
                        tsSettings.AddTargetSystem(TargetSystemInfoHelper.Base);
                    }

                    if (tsSettings.RegisterAuthorization)
                    {
                        tsSettings.AddTargetSystem<IAuthorizationBLLContext, Authorization.Domain.PersistentDomainObjectBase>(
                            TargetSystemInfoHelper.Authorization);
                    }

                    if (tsSettings.RegisterConfiguration)
                    {
                        tsSettings.AddTargetSystem<IConfigurationBLLContext, Configuration.Domain.PersistentDomainObjectBase>(
                            TargetSystemInfoHelper.Configuration);
                    }

                    tsSettings.Initialize(services);
                });

        public IBssFrameworkBuilder SetSubscriptionAssembly(Assembly assembly) =>
            builder.AddServices(services => services.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(assembly)));

        public IBssFrameworkBuilder SetNotificationEmployee<TEmployee>()
            where TEmployee : class, IEmployee =>
            builder.AddServices(services => services.AddScoped<IEmployeeSource, EmployeeSource<TEmployee>>());

        public IBssFrameworkBuilder SetNotificationDefaultMailSenderContainer<TDefaultMailSenderContainer>()
            where TDefaultMailSenderContainer : class, IDefaultMailSenderContainer =>
            builder.AddServices(services => services.AddSingleton<IDefaultMailSenderContainer, TDefaultMailSenderContainer>());
    }
}
