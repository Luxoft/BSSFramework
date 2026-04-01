using CommonFramework;

using Framework.Application.Domain;
using Framework.BLL;
using Framework.BLL.Services;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Core.TypeResolving;
using Framework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class TargetSystemRootSettings : ITargetSystemRootSettings
{
    private readonly List<Action<IServiceCollection>> registerActions = [];

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
        ISecurityServiceContainer<IRootSecurityService>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
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
        ISecurityServiceContainer<IRootSecurityService>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        this.AddTargetSystem(info);

        this.registerActions.Add(services => services.AddScopedFromLazyInterfaceImplement<ITargetSystemService, TargetSystemService<TBLLContext, TPersistentDomainObjectBase>>());

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
