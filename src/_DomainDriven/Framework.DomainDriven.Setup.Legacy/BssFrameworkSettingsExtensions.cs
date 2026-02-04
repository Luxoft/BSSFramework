using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;
using Framework.Events.Legacy;
using Framework.Notification;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class BssFrameworkSettingsExtensions
{
    extension(IBssFrameworkSettings settings)
    {
        public IBssFrameworkSettings AddSystemConstant(Type systemConstantContainerType)
        {
            return settings.AddServices(sc => sc.AddSingleton(new SystemConstantInfo(systemConstantContainerType)));
        }

        public IBssFrameworkSettings AddSubscriptionManager<TSubscriptionManager>()
            where TSubscriptionManager : class, IEventOperationReceiver
        {
            return settings.AddServices(sc => sc.RegisterSubscriptionManagers(setup => setup.Add<TSubscriptionManager>()));
        }

        public IBssFrameworkSettings AddLegacyGenericServices()
        {
            return settings.AddServices(sc => sc.RegisterLegacyGenericServices());
        }

        public IBssFrameworkSettings AddContextEvaluators()
        {
            return settings.AddServices(sc => sc.RegisterContextEvaluators());
        }

        public IBssFrameworkSettings AddBLLSystem<TBLLContextDecl, TBLLContextImpl>(Action<BLLSystemSettings>? setupAction = null)
            where TBLLContextImpl : TBLLContextDecl
        {
            return settings.AddServices(sc => sc.RegisterBLLSystem<TBLLContextDecl, TBLLContextImpl>(setupAction));
        }

        public IBssFrameworkSettings AddConfigurationTargetSystems(Action<ITargetSystemRootSettings> setupAction)
        {
            return settings.AddServices(
                sc =>
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

                    tsSettings.Initialize(sc);
                });
        }

        public IBssFrameworkSettings SetSubscriptionAssembly(Assembly assembly) =>
            settings.AddServices(sc => sc.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(assembly)));

        public IBssFrameworkSettings SetNotificationEmployee<TEmployee>()
            where TEmployee : class, IEmployee =>
            settings.AddServices(sc => sc.AddScoped<IEmployeeSource, EmployeeSource<TEmployee>>());

        public IBssFrameworkSettings SetNotificationDefaultMailSenderContainer<TDefaultMailSenderContainer>()
            where TDefaultMailSenderContainer : class, IDefaultMailSenderContainer =>
            settings.AddServices(sc => sc.AddSingleton<IDefaultMailSenderContainer, TDefaultMailSenderContainer>());

        public IBssFrameworkSettings SetDTOMapping<TDTOMappingService, TDTOMappingServiceImpl, TPersistentDomainObjectBase, TEventDTOBase>()
            where TDTOMappingService : class
            where TDTOMappingServiceImpl : class, TDTOMappingService =>
            settings.AddServices(sc => sc.AddScoped<TDTOMappingService, TDTOMappingServiceImpl>()
                                         .AddScoped<IDomainEventDTOMapper<TPersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<TPersistentDomainObjectBase,
                                             TDTOMappingService, TEventDTOBase>>());

        private IBssFrameworkSettings AddServices(Action<IServiceCollection> setupAction)
        {
            return settings.AddExtensions(new BssFrameworkExtension(setupAction));
        }
    }
}
