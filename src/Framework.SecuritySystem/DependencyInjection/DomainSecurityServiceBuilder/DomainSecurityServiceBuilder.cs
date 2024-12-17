using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem.ProviderFactories;
using Framework.SecuritySystem.SecurityRuleInfo;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public abstract class DomainSecurityServiceBuilder
{
    public abstract Type DomainType { get; }

    public abstract void Register(IServiceCollection services, bool addSelfRelativePath);
}

internal class DomainSecurityServiceBuilder<TDomainObject> : DomainSecurityServiceBuilder, IDomainSecurityServiceBuilder<TDomainObject>
    where TDomainObject : IIdentityObject<Guid>
{
    private readonly List<Type> injectorTypes = [];

    private readonly Dictionary<SecurityRule.ModeSecurityRule, DomainSecurityRule> domainObjectSecurityDict = [];

    private SecurityPath<TDomainObject>? securityPath;

    private (Type Type, object Instance)? relativePathData;

    private Type? customServiceType;

    private Type? dependencyInjectorType;

    public override Type DomainType { get; } = typeof(TDomainObject);

    public override void Register(IServiceCollection services, bool addSelfRelativePath)
    {
        foreach (var (modeSecurityRule, implementedSecurityRule) in this.domainObjectSecurityDict)
        {
            services.AddSingleton(new DomainModeSecurityRuleInfo(modeSecurityRule.ToDomain(this.DomainType), implementedSecurityRule));
        }

        if (this.securityPath != null)
        {
            services.AddSingleton(this.securityPath);
        }

        if (this.relativePathData is { } pair)
        {
            services.AddSingleton(pair.Type, pair.Instance);
        }

        if (addSelfRelativePath)
        {
            services.AddSingleton<IRelativeDomainPathInfo<TDomainObject, TDomainObject>, SelfRelativeDomainPathInfo<TDomainObject>>();
        }

        services.AddScoped(typeof(IDomainSecurityService<TDomainObject>), this.customServiceType ?? typeof(DomainSecurityService<TDomainObject>));
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetMode(SecurityRule.ModeSecurityRule modeSecurityRule, DomainSecurityRule implementedSecurityRule)
    {
        this.domainObjectSecurityDict[modeSecurityRule] = implementedSecurityRule;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> newSecurityPath)
    {
        this.securityPath = newSecurityPath;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>()
    {
        this.dependencyInjectorType =
        this.dependencyServiceType = typeof(DependencyDomainSecurityService<TDomainObject, TSource>);

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> relativeDomainPath)
    {
        this.SetDependency<TSource>();

        this.relativePathData =
            (typeof(IRelativeDomainPathInfo<TDomainObject, TSource>),
                new RelativeDomainPathInfo<TDomainObject, TSource>(relativeDomainPath));

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetUntypedDependency<TSource>()
    {
        this.dependencyServiceType = typeof(UntypedDependencyDomainSecurityService<,>).MakeGenericType(typeof(TDomainObject), typeof(TSource));

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>
    {
        this.customServiceType = typeof(TDomainSecurityService);

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> AddInjector<TSecurityFunctor>()
        where TSecurityFunctor : ISecurityProviderInjector<TDomainObject>
    {
        this.functorTypes.Add(typeof(TSecurityFunctor));

        return this;
    }
}
