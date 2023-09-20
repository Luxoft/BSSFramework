using System.Linq.Expressions;

using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public class AuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> : IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent>, IAuthorizationSystemDomainServiceBuilder
    where TDomainObject : IIdentityObject<TIdent>
{
    public SecurityOperation ViewSecurityOperation { get; private set; }

    public SecurityOperation EditSecurityOperation { get; private set; }

    public SecurityPath<TDomainObject> SecurityPath { get; private set; }

    public object DependencySourcePath { get; private set; }

    public Type CustomServiceType { get; private set; }

    public Type DependencyServiceType { get; private set; }


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

        if (this.DependencySourcePath != null)
        {
            services.AddSingleton(this.DependencySourcePath.GetType(), this.DependencySourcePath);
        }

        services.AddScoped(typeof(IDomainSecurityService<TDomainObject>), sp => ActivatorUtilities.CreateInstance(sp, this.GetDomainServiceType()));
    }

    private Type GetDomainServiceType()
    {
        if (this.CustomServiceType != null)
        {
            return this.CustomServiceType;
        }
        else if (this.DependencyServiceType != null)
        {
            return this.DependencyServiceType;
        }
        else if (this.SecurityPath != null)
        {
            return typeof(ContextDomainSecurityService<TDomainObject, TIdent>);
        }
        else
        {
            return typeof(NonContextDomainSecurityService<TDomainObject, TIdent>);
        }
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetView(SecurityOperation securityOperation)
    {
        this.ViewSecurityOperation = securityOperation;

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetEdit(SecurityOperation securityOperation)
    {
        this.EditSecurityOperation = securityOperation;

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetPath(SecurityPath<TDomainObject> securityPath)
    {
        this.SecurityPath = securityPath;

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> dependencyPath)
    {
        this.DependencyServiceType = typeof(DependencyDomainSecurityService<TDomainObject, TSource>);
        this.DependencySourcePath = new DependencyDomainSecurityServicePath<TDomainObject, TSource>(dependencyPath);

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetUntypedDependency<TSource>()
        where TSource : class, IIdentityObject<TIdent>
    {
        this.DependencyServiceType = typeof(UntypedDependencyDomainSecurityService<TDomainObject, TSource, TIdent>);

        return this;
    }

    public IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>
    {
        this.CustomServiceType = typeof(TDomainSecurityService);

        return this;
    }
}
