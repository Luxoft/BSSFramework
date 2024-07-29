using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
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
                                        var tsSettings = new TargetSystemRootSettings();

                                        setupAction.Invoke(tsSettings);

                                        if (tsSettings.RegisterBase)
                                        {
                                            tsSettings.AddTargetSystem(TargetSystemInfoHelper.Base);
                                        }

                                        if (tsSettings.RegisterAuthorization)
                                        {
                                            tsSettings
                                                .AddTargetSystem<IAuthorizationBLLContext,
                                                    Framework.Authorization.Domain.PersistentDomainObjectBase>(
                                                    TargetSystemInfoHelper.Authorization);
                                        }

                                        if (tsSettings.RegisterConfiguration)
                                        {
                                            tsSettings
                                                .AddTargetSystem<IConfigurationBLLContext,
                                                    Framework.Configuration.Domain.PersistentDomainObjectBase>(
                                                    TargetSystemInfoHelper.Configuration);
                                        }

                                        tsSettings.Initialize(sc);
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

    bool RegisterAuthorization { get; set; }

    bool RegisterConfiguration { get; set; }

    ITargetSystemRootSettings AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(string name, Guid id, bool isMain, bool isRevision, IEnumerable<DomainTypeInfo> domainTypes)
        where TBLLContext : class, ITypeResolverContainer<string>,
        ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>;
}

public class TargetSystemRootSettings : ITargetSystemRootSettings
{
    private readonly List<Action<IServiceCollection>> registerActions = new();

    public bool RegisterBase { get; set; }

    public bool RegisterAuthorization { get; set; }

    public bool RegisterConfiguration { get; set; }

    public ITargetSystemRootSettings AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(
        string name,
        Guid id,
        bool isMain,
        bool isRevision,
        IEnumerable<DomainTypeInfo> domainTypes)
        where TBLLContext : class, ITypeResolverContainer<string>,
        ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        var info = new TargetSystemInfo<TPersistentDomainObjectBase>(
            name,
            id,
            isMain,
            isRevision,
            domainTypes.ToList());

        return this.AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(info);
    }

    public ITargetSystemRootSettings AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(TargetSystemInfo<TPersistentDomainObjectBase> info)
        where TBLLContext : class, ITypeResolverContainer<string>,
        ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        this.AddTargetSystem(info);

        this.registerActions.Add(sc => sc.AddScoped<ITargetSystemService, TargetSystemService<TBLLContext, TPersistentDomainObjectBase>>());

        return this;
    }

    public ITargetSystemRootSettings AddTargetSystem<TPersistentDomainObjectBase>(TargetSystemInfo<TPersistentDomainObjectBase> info)
    {
        this.registerActions.Add(
            sc =>
            {
                sc.AddSingleton(info);
                sc.AddSingleton<TargetSystemInfo>(info);
            });

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<ITargetSystemInitializer, TargetSystemInitializer>();

        this.registerActions.Foreach(action => action(services));
    }
}
