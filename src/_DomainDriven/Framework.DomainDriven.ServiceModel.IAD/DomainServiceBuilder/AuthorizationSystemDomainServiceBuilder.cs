using System.Linq.Expressions;

using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public class AuthorizationSystemDomainServiceBuilder<TDomainObject> : IAuthorizationSystemDomainServiceBuilder<TDomainObject>, IAuthorizationSystemDomainServiceBuilder
{
    public SecurityOperation ViewSecurityOperation { get; private set; }

    public SecurityOperation EditSecurityOperation { get; private set; }

    public SecurityPath<TDomainObject> SecurityPath { get; private set; }

    public Type DependencySourceType { get; private set; }

    public object DependencySourcePath { get; private set; }

    public Type CustomServiceType { get; private set; }


    public void Register(IServiceCollection services)
    {
        if (this.ViewSecurityOperation != null || this.EditSecurityOperation != null)
        {
            services.AddSingleton(new DomainObjectSecurityOperationInfo(typeof(TDomainObject), this.ViewSecurityOperation, this.EditSecurityOperation));
        }

        if (this.SecurityPath != null)
        {
            services.AddSingleton(this.SecurityPath);
        }

        services.AddScoped(typeof(IDomainSecurityService<TDomainObject>), this.GetDomainServiceType());
    }

    private Type GetDomainServiceType()
    {
        if (this.CustomServiceType != null)
        {
            return this.CustomServiceType;
        }
        else if (this.DependencySourceType != null)
        {
            if (this.DependencySourcePath == null)
            {
                return typeof(UntypedDependencySecurityProvider<,,>).MakeGenericType(typeof(TDomainObject), this.DependencySourceType, typeof(Guid));
            }
            else
            {
                return typeof(DependencySecurityProvider<,>).MakeGenericType(typeof(TDomainObject), this.DependencySourceType);
            }
        }
        else if (this.SecurityPath != null)
        {
            return typeof(ContextSecurityOperation<Guid>);
        }
        else
        {
            return typeof(NonContextSecurityOperation<Guid>);
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
        this.DependencySourcePath = new DependencyDomainSecurityServicePath<TDomainObject, TSource>(dependencyPath);

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
