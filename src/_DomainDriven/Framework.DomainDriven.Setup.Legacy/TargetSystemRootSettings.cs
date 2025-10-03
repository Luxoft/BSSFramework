using CommonFramework;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public class TargetSystemRootSettings : ITargetSystemRootSettings
{
    private readonly List<Action<IServiceCollection>> registerActions = new();

    public bool RegisterBase { get; set; } = true;

    public bool RegisterAuthorization { get; set; } = true;

    public bool RegisterConfiguration { get; set; } = true;

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

        this.registerActions.Add(sc => sc.AddScopedFromLazyInterfaceImplement<ITargetSystemService, TargetSystemService<TBLLContext, TPersistentDomainObjectBase>>());

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
