using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;
using Framework.Events.Legacy;
using Framework.Notification;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class BssFrameworkSettingsExtensions
{
    public static IBssFrameworkSettings AddSubscriptionManager<TSubscriptionManager>(this IBssFrameworkSettings settings)
        where TSubscriptionManager : class, IEventOperationReceiver
    {
        return settings.AddServices(sc => sc.RegisterSubscriptionManagers(setup => setup.Add<TSubscriptionManager>()));
    }

    public static IBssFrameworkSettings AddLegacyGenericServices(this IBssFrameworkSettings settings)
    {
        return settings.AddServices(sc => sc.RegisterLegacyGenericServices());
    }

    public static IBssFrameworkSettings AddContextEvaluators(this IBssFrameworkSettings settings)
    {
        return settings.AddServices(sc => sc.RegisterContextEvaluators());
    }

    public static IBssFrameworkSettings AddBLLSystem<TBLLContextDecl, TBLLContextImpl>(this IBssFrameworkSettings settings, Action<BLLSystemSettings> setupAction = null)
        where TBLLContextImpl : TBLLContextDecl
    {
        return settings.AddServices(sc => sc.RegisterBLLSystem<TBLLContextDecl, TBLLContextImpl>(setupAction));
    }

    public static IBssFrameworkSettings AddLegacyDatabaseSettings(this IBssFrameworkSettings settings)
    {
        return settings.AddServices(sc => sc.AddLegacyDatabaseSettings());
    }

    public static IBssFrameworkSettings AddConfigurationTargetSystems(this IBssFrameworkSettings settings, Action<ITargetSystemRootSettings> setupAction)
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

    public static IBssFrameworkSettings SetSubscriptionAssembly(this IBssFrameworkSettings settings, Assembly assembly) =>
        settings.AddServices(sc => sc.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(assembly)));

    public static IBssFrameworkSettings SetNotificationEmployee<TEmployee>(this IBssFrameworkSettings settings)
        where TEmployee : class, IEmployee =>
        settings.AddServices(sc => sc.AddScoped<IEmployeeSource, EmployeeSource<TEmployee>>());

    public static IBssFrameworkSettings SetNotificationDefaultMailSenderContainer<TDefaultMailSenderContainer>(this IBssFrameworkSettings settings)
        where TDefaultMailSenderContainer : class, IDefaultMailSenderContainer =>
        settings.AddServices(sc => sc.AddSingleton<IDefaultMailSenderContainer, TDefaultMailSenderContainer>());

    public static IBssFrameworkSettings SetDTOMapping<TDTOMappingService, TDTOMappingServiceImpl, TPersistentDomainObjectBase, TEventDTOBase>(this IBssFrameworkSettings settings)
        where TDTOMappingService : class
        where TDTOMappingServiceImpl : class, TDTOMappingService =>
        settings.AddServices(sc => sc.AddScoped<TDTOMappingService, TDTOMappingServiceImpl>()
                                     .AddScoped<IDomainEventDTOMapper<TPersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<TPersistentDomainObjectBase,
                                         TDTOMappingService, TEventDTOBase>>());

    private static IBssFrameworkSettings AddServices(this IBssFrameworkSettings settings, Action<IServiceCollection> setupAction)
    {
        return settings.AddExtensions(new BssFrameworkExtension(setupAction));
    }
}
