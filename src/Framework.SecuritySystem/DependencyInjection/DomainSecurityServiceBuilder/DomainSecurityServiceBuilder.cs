using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem.SecurityRuleInfo;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public abstract class DomainSecurityServiceBuilder : IDomainSecurityServiceBuilder
{
    public abstract Type DomainType { get; }

    public abstract void Register(IServiceCollection services);
}

internal class DomainSecurityServiceBuilder<TDomainObject> : DomainSecurityServiceBuilder, IDomainSecurityServiceBuilder<TDomainObject>
    where TDomainObject : IIdentityObject<Guid>
{
    private readonly List<Type> functorTypes = [];

    private readonly Dictionary<SecurityRule.ModeSecurityRule, DomainSecurityRule> domainObjectSecurityDict = [];

    private SecurityPath<TDomainObject>? securityPath;

    private (Type Type, object Instance)? relativePathData;

    private Type? customServiceType;

    private Type? dependencyServiceType;

    public override Type DomainType { get; } = typeof(TDomainObject);

    public override void Register(IServiceCollection services)
    {
        foreach (var domainObjectSecurityPair in this.domainObjectSecurityDict)
        {
            services.AddSingleton(new DomainModeSecurityRuleInfo(domainObjectSecurityPair.Key.ToDomain(this.DomainType), domainObjectSecurityPair.Value));
        }

        if (this.securityPath != null)
        {
            services.AddSingleton(this.securityPath);
        }

        if (this.relativePathData is { } pair)
        {
            services.AddSingleton(pair.Type, pair.Instance);
        }

        foreach (var (decl, impl) in this.GetRegisterDomainSecurityService())
        {
            if (decl == null)
            {
                services.AddScoped(impl);
            }
            else
            {
                services.AddScoped(decl, impl);
            }
        }
    }

    private IEnumerable<(Type? Decl, Type Impl)> GetRegisterDomainSecurityService()
    {
        var baseServiceType = typeof(IDomainSecurityService<TDomainObject>);

        var actualCustomServiceType = this.customServiceType ?? this.dependencyServiceType;

        var functorTypeDecl = typeof(IOverrideSecurityProviderFunctor<TDomainObject>);

        var realFunctorTypes = this.functorTypes.Where(f => f.HasInterfaceMethodOverride(functorTypeDecl)).ToList();

        if (realFunctorTypes.Any())
        {
            foreach (var functorType in realFunctorTypes)
            {
                yield return (functorTypeDecl, functorType);
            }

            var withFunctorActualCustomServiceType = actualCustomServiceType ?? typeof(ContextDomainSecurityService<TDomainObject>);

            yield return (null, withFunctorActualCustomServiceType);

            var withWrappedFunctorServiceType = typeof(DomainSecurityServiceWithFunctor<,>).MakeGenericType(
                withFunctorActualCustomServiceType,
                typeof(TDomainObject));

            yield return (baseServiceType, withWrappedFunctorServiceType);
        }
        else if (actualCustomServiceType != null)
        {
            yield return (baseServiceType, actualCustomServiceType);
        }
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetView(DomainSecurityRule securityRule)
    {
        this.domainObjectSecurityDict[SecurityRule.View] = securityRule;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetEdit(DomainSecurityRule securityRule)
    {
        this.domainObjectSecurityDict[SecurityRule.Edit] = securityRule;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> newSecurityPath)
    {
        this.securityPath = newSecurityPath;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>()
    {
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

    public IDomainSecurityServiceBuilder<TDomainObject> Override<TSecurityFunctor>()
        where TSecurityFunctor : IOverrideSecurityProviderFunctor<TDomainObject>
    {
        this.functorTypes.Add(typeof(TSecurityFunctor));

        return this;
    }
}
