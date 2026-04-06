using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class BssFrameworkSettingsExtensions
{
    extension(IBssFrameworkSetup builder)
    {
        public IBssFrameworkSetup AddSystemConstant(Type systemConstantContainerType) => builder.AddServices(services => services.AddSingleton(new SystemConstantInfo(systemConstantContainerType)));

        public IBssFrameworkSetup AddLegacyDefaultGenericServices() => builder.AddServices(services => services.AddLegacyDefaultGenericServices());

        public IBssFrameworkSetup AddConfigurationTargetSystems(Action<ITargetSystemSetup> setupAction) =>
            builder.AddServices(
                services =>
                {
                    var tsSettings = new TargetSystemSetup();

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

        public IBssFrameworkSetup SetSubscriptionAssembly(Assembly assembly) =>
            builder.AddServices(services => services.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(assembly)));

        public IBssFrameworkSetup SetNotificationEmployee<TEmployee>()
            where TEmployee : class, IEmployee =>
            builder.AddServices(services => services.AddScoped<IEmployeeSource, EmployeeSource<TEmployee>>());

        public IBssFrameworkSetup SetNotificationDefaultMailSenderContainer<TDefaultMailSenderContainer>()
            where TDefaultMailSenderContainer : class, IDefaultMailSenderContainer =>
            builder.AddServices(services => services.AddSingleton<IDefaultMailSenderContainer, TDefaultMailSenderContainer>());
    }
}
