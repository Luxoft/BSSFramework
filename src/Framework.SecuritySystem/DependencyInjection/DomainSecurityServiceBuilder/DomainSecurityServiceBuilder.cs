using System.Linq.Expressions;

using Framework.Persistent;
using Framework.SecuritySystem.Expanders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public abstract class DomainSecurityServiceBuilder : IDomainSecurityServiceBuilder
{
    public abstract Type DomainType { get; }

    public abstract void Register(IServiceCollection services);
}

internal class DomainSecurityServiceBuilder<TDomainObject, TIdent> : DomainSecurityServiceBuilder, IDomainSecurityServiceBuilder<TDomainObject>
    where TDomainObject : IIdentityObject<TIdent>
{
    private readonly List<Type> securityFunctorTypes = new();

    public DomainSecurityRule? ViewRule { get; private set; }

    public DomainSecurityRule? EditRule { get; private set; }

    public SecurityPath<TDomainObject> SecurityPath { get; private set; } = SecurityPath<TDomainObject>.Empty;

    public (Type Type, object Instance)? DependencySourcePathData { get; private set; }

    public Type? CustomServiceType { get; private set; }

    public Type? DependencyServiceType { get; private set; }

    public override Type DomainType { get; } = typeof(TDomainObject);

    public override void Register(IServiceCollection services)
    {
        if (this.ViewRule != null || this.EditRule != null)
        {
            services.AddSingleton(
                new DomainObjectSecurityModeInfo(typeof(TDomainObject), this.ViewRule, this.EditRule));
        }

        services.AddSingleton(this.SecurityPath);

        if (this.DependencySourcePathData is { } pair)
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

        var actualCustomServiceType = this.CustomServiceType ?? this.DependencyServiceType;

        if (this.securityFunctorTypes.Any())
        {
            foreach (var securityFunctorType in this.securityFunctorTypes)
            {
                yield return (typeof(IOverrideSecurityProviderFunctor<TDomainObject>), securityFunctorType);
            }

            var actualCustomServiceTypeWithFunctor = actualCustomServiceType ?? typeof(ContextDomainSecurityService<TDomainObject>);

            yield return (null, actualCustomServiceTypeWithFunctor);

            var wrappedFunctorType = typeof(DomainSecurityServiceWithFunctor<,>).MakeGenericType(
                actualCustomServiceTypeWithFunctor,
                typeof(TDomainObject));

            yield return (baseServiceType, wrappedFunctorType);
        }
        else if (actualCustomServiceType != null)
        {
            yield return (baseServiceType, actualCustomServiceType);
        }
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetView(DomainSecurityRule securityRule)
    {
        this.ViewRule = securityRule;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetEdit(DomainSecurityRule securityRule)
    {
        this.EditRule = securityRule;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath)
    {
        this.SecurityPath = securityPath;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>()
    {
        this.DependencyServiceType = typeof(DependencyDomainSecurityService<TDomainObject, TSource>);

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> relativeDomainPath)
    {
        this.SetDependency<TSource>();

        this.DependencySourcePathData =
            (typeof(IRelativeDomainPathInfo<TDomainObject, TSource>),
                new RelativeDomainPathInfo<TDomainObject, TSource>(relativeDomainPath));

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetUntypedDependency<TSource>()
    {
        this.DependencyServiceType = typeof(UntypedDependencyDomainSecurityService<,,>).MakeGenericType(typeof(TDomainObject), typeof(TSource), typeof(TIdent));

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>
    {
        this.CustomServiceType = typeof(TDomainSecurityService);

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> Override<TSecurityFunctor>()
        where TSecurityFunctor : IOverrideSecurityProviderFunctor<TDomainObject>
    {
        this.securityFunctorTypes.Add(typeof(TSecurityFunctor));

        return this;
    }
}
