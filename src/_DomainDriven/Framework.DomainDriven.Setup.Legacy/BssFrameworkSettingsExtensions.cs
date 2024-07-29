using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;
using Framework.Events.Legacy;
using Framework.Persistent;

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

    public static IBssFrameworkSettings AddConfigurationTargetSystems(this IBssFrameworkSettings settings, Action<ITargetSystemRootSettings> setupAction)
    {
        return settings.AddServices(sc =>
                                    {
                                        var tsSettings = new TargetSystemRootSettings(sc);

                                        setupAction.Invoke(tsSettings);

                                        if (tsSettings.RegisterBase)
                                        {
                                            tsSettings.AddTargetSystem<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>(false, TargetSystemHelper.AuthorizationName);
                                            tsSettings.AddTargetSystem<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>(false, TargetSystemHelper.ConfigurationName);
                                        }

                                        tsSettings.Initialize();
                                    });
    }

    private static IBssFrameworkSettings AddServices(this IBssFrameworkSettings settings, Action<IServiceCollection> setupAction)
    {
        return settings.AddExtensions(new BssFrameworkExtension(setupAction));
    }
}

public interface ITargetSystemRootSettings
{
    bool RegisterBase { get; set; }

    ITargetSystemRootSettings AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(bool isMain, bool isRevision, string name = null)
        where TBLLContext : class, ITypeResolverContainer<string>,
        ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>;
}

public class TargetSystemRootSettings(IServiceCollection services) : ITargetSystemRootSettings
{
    public bool RegisterBase { get; set; }

    public ITargetSystemRootSettings AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(bool isMain, bool isRevision, string name = null)
        where TBLLContext : class, ITypeResolverContainer<string>,
        ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        var info = new TargetSystemInfo<TPersistentDomainObjectBase>(isMain, isRevision, name ?? typeof(TPersistentDomainObjectBase).GetTargetSystemName() );

        services.AddSingleton<>()
        services.AddScoped<ITargetSystemService, TargetSystemService<TBLLContext, TPersistentDomainObjectBase>>();

        return this;
    }

    public void Initialize()
    {

    }
}


public interface ITargetSystemInitializer
{
    void Init();
}
