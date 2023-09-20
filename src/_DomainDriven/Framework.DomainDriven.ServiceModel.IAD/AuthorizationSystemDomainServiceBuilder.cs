using System.Linq.Expressions;

using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IAuthorizationSystemRootDomainServiceBuilder
{
    IAuthorizationSystemRootDomainServiceBuilder Add<TDomainObject>(Action<IAuthorizationSystemDomainServiceBuilder<TDomainObject>> setup);
}

public class AuthorizationSystemRootDomainServiceBuilder : IAuthorizationSystemRootDomainServiceBuilder
{
    public IAuthorizationSystemRootDomainServiceBuilder Add<TDomainObject>(Action<IAuthorizationSystemDomainServiceBuilder<TDomainObject>> setup)
    {
        var builder = new AuthorizationSystemDomainServiceBuilder<TDomainObject>();

        setup(builder);

        return this;
    }
}

public interface IAuthorizationSystemDomainServiceBuilder<TDomainObject>
{
    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetView(SecurityOperation securityOperation);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetEdit(SecurityOperation securityOperation);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> securityPath);

    /// <summary>
    /// For projection
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetUntypedDependency<TSource>();

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>;
}

public class AuthorizationSystemDomainServiceBuilder<TDomainObject> : IAuthorizationSystemDomainServiceBuilder<TDomainObject>
{
    public SecurityOperation ViewSecurityOperation { get; private set; }

    public SecurityOperation EditSecurityOperation { get; private set; }

    public SecurityPath<TDomainObject> SecurityPath { get; private set; }

    public Type DependencySourceType { get; private set; }

    public Expression DependencySourcePath { get; private set; }

    public Type CustomServiceType { get; private set; }


    public void RegisterDomainService(IServiceCollection services)
    {
        if (this.CustomServiceType != null)
        {
            services.AddScoped(typeof(IDomainSecurityService<TDomainObject>), this.CustomServiceType);
        }
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetView(SecurityOperation securityOperation)
    {
        this.ViewSecurityOperation = securityOperation;

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetEdit(SecurityOperation securityOperation)
    {
        this.EditSecurityOperation = securityOperation;

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath)
    {
        this.SecurityPath = securityPath;

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> dependencyPath)
    {
        this.DependencySourceType = typeof(TSource);
        this.DependencySourcePath = dependencyPath;

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetUntypedDependency<TSource>()
    {
        this.DependencySourceType = typeof(TSource);

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>
    {
        this.CustomServiceType = typeof(TDomainSecurityService);

        return this;
    }
}
