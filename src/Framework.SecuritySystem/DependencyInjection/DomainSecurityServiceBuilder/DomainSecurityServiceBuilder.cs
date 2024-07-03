using System.Linq.Expressions;

using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

internal class DomainSecurityServiceBuilder<TDomainObject, TIdent> : IDomainSecurityServiceBuilder<TDomainObject>, IDomainSecurityServiceBuilder
    where TDomainObject : IIdentityObject<TIdent>
{
    private readonly List<Type> securityFunctorTypes = new ();

    public List<SecurityRule> ViewRules { get; } = new();

    public List<SecurityRule> EditRules { get; } = new();

    public SecurityPath<TDomainObject> SecurityPath { get; private set; } = SecurityPath<TDomainObject>.Empty;

    public object DependencySourcePathInfo { get; private set; }

    public Type DependencySourcePathType { get; private set; }

    public Type CustomServiceType { get; private set; }

    public Type DependencyServiceType { get; private set; }


    public void Register(IServiceCollection services)
    {
        if (this.ViewRules.Any() || this.EditRules.Any())
        {
            services.AddSingleton(
                new DomainObjectSecurityModeInfo(typeof(TDomainObject), this.ViewRules, this.EditRules));
        }

        services.AddSingleton(this.SecurityPath);

        if (this.DependencySourcePathInfo != null)
        {
            services.AddSingleton(this.DependencySourcePathType, this.DependencySourcePathInfo);
        }

        var originalDomainServiceType = this.GetOriginalDomainServiceType();
        Type registerDomainServiceType;

        if (this.securityFunctorTypes.Any())
        {
            foreach (var securityFunctorType in this.securityFunctorTypes)
            {
                services.AddScoped(typeof(IOverrideSecurityProviderFunctor<TDomainObject>), securityFunctorType);
            }

            services.AddScoped(originalDomainServiceType);

            registerDomainServiceType = typeof(DomainSecurityServiceWithFunctor<,>).MakeGenericType(originalDomainServiceType, typeof(TDomainObject));
        }
        else
        {
            registerDomainServiceType = originalDomainServiceType;
        }

        services.AddScoped(typeof(IDomainSecurityService<TDomainObject>), registerDomainServiceType);
    }

    private Type GetOriginalDomainServiceType()
    {
        if (this.CustomServiceType != null)
        {
            return this.CustomServiceType;
        }
        else if (this.DependencyServiceType != null)
        {
            return this.DependencyServiceType;
        }
        else
        {
            return typeof(ContextDomainSecurityService<TDomainObject>);
        }
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetView(SecurityRule securityRule)
    {
        this.ViewRules.Add(securityRule);

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetView<TSecurityProvider>()
        where TSecurityProvider : ISecurityProvider<TDomainObject>
    {
        this.ViewRules.Add(new SecurityRule.CustomProviderSecurityRule(typeof(TSecurityProvider)));

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetEdit(SecurityRule securityRule)
    {
        this.EditRules.Add(securityRule);

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetEdit<TSecurityProvider>()
        where TSecurityProvider : ISecurityProvider<TDomainObject>
    {
        this.EditRules.Add(new SecurityRule.CustomProviderSecurityRule(typeof(TSecurityProvider)));

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath)
    {
        this.SecurityPath = securityPath;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> dependencyPath)
    {
        this.DependencyServiceType = typeof(DependencyDomainSecurityService<TDomainObject, TSource>);
        this.DependencySourcePathInfo = new RelativeDomainPathInfo<TDomainObject, TSource>(dependencyPath);
        this.DependencySourcePathType = typeof(IRelativeDomainPathInfo<TDomainObject, TSource>);

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
