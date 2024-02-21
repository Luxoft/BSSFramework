using System.Linq.Expressions;

using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

internal class DomainSecurityServiceBuilder<TDomainObject, TIdent> : IDomainSecurityServiceBuilder<TDomainObject>, IDomainSecurityServiceBuilder
    where TDomainObject : IIdentityObject<TIdent>
{
    private readonly List<Type> securityFunctorTypes = new ();

    public SecurityOperation ViewSecurityOperation { get; private set; }

    public SecurityOperation EditSecurityOperation { get; private set; }

    public SecurityPath<TDomainObject> SecurityPath { get; private set; } = SecurityPath<TDomainObject>.Empty;

    public object DependencySourcePath { get; private set; }

    public Type CustomServiceType { get; private set; }

    public Type DependencyServiceType { get; private set; }


    public void Register(IServiceCollection services)
    {
        if (this.ViewSecurityOperation != null || this.EditSecurityOperation != null)
        {
            services.AddSingleton(
                new DomainObjectSecurityOperationInfo(typeof(TDomainObject), this.ViewSecurityOperation, this.EditSecurityOperation));
        }

        services.AddSingleton(this.SecurityPath);

        if (this.DependencySourcePath != null)
        {
            services.AddSingleton(this.DependencySourcePath.GetType(), this.DependencySourcePath);
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
            return typeof(ContextDomainSecurityService<TDomainObject, TIdent>);
        }
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetView(SecurityOperation securityOperation)
    {
        this.ViewSecurityOperation = securityOperation;

        return this;
    }

    public IDomainSecurityServiceBuilder<TDomainObject> SetEdit(SecurityOperation securityOperation)
    {
        this.EditSecurityOperation = securityOperation;

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
        this.DependencySourcePath = new DependencyDomainSecurityServicePath<TDomainObject, TSource>(dependencyPath);

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
