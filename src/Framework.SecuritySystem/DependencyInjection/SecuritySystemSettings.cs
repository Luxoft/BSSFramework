using System.Linq.Expressions;

using Framework.DependencyInjection;
using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.SecurityAccessor;
using Framework.SecuritySystem.Services;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public class SecuritySystemSettings : ISecuritySystemSettings
{
    public List<Action<IServiceCollection>> RegisterActions { get; } = new();

    public Action<IServiceCollection> RegisterUserSourceAction { get; private set; } = _ => { };

    public Action<IServiceCollection> RegisterRunAsManagerAction { get; private set; } = _ => { };

    public bool InitializeAdministratorRole { get;  set; } = true;

    public SecurityRuleCredential DefaultSecurityRuleCredential { get; private set; } = SecurityRuleCredential.CurrentUserWithRunAs;

    public Type AccessDeniedExceptionServiceType { get; private set; } = typeof(AccessDeniedExceptionService);

    public Type CurrentUserType { get; private set; } = typeof(CurrentUser);

    public Type? SecurityAccessorInfinityStorageType { get; private set; }

    public ISecuritySystemSettings AddSecurityContext<TSecurityContext>(
        Guid ident,
        string? name,
        Func<TSecurityContext, string>? displayFunc)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        return this.AddSecurityContext(b => b.Add(ident, name, displayFunc));
    }

    public ISecuritySystemSettings AddSecurityContext(Action<ISecurityContextInfoBuilder> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterSecurityContextSource(setup));

        return this;
    }

    public ISecuritySystemSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterDomainSecurityServices(setup));

        return this;
    }

    public ISecuritySystemSettings AddSecurityRole(SecurityRole securityRole, SecurityRoleInfo info)
    {
        this.RegisterActions.Add(sc => this.AddSecurityRole(sc, new FullSecurityRole(securityRole.Name, info)));

        return this;
    }

    public ISecuritySystemSettings AddSecurityRule(DomainSecurityRule.SecurityRuleHeader header, DomainSecurityRule implementation)
    {
        this.RegisterActions.Add(sc => sc.AddSingleton(new SecurityRuleFullInfo(header, implementation)));

        return this;
    }

    public ISecuritySystemSettings AddSecurityOperation(SecurityOperation securityOperation, SecurityOperationInfo info)
    {
        this.RegisterActions.Add(sc => sc.AddSingleton(new FullSecurityOperation(securityOperation, info)));

        return this;
    }

    public ISecuritySystemSettings AddPermissionSystem<TPermissionSystemFactory>()
        where TPermissionSystemFactory : class, IPermissionSystemFactory
    {
        this.RegisterActions.Add(sc => sc.AddScoped<IPermissionSystemFactory, TPermissionSystemFactory>());

        return this;
    }

    public ISecuritySystemSettings AddPermissionSystem(Func<IServiceProvider, IPermissionSystemFactory> getFactory)
    {
        this.RegisterActions.Add(sc => sc.AddScoped(getFactory));

        return this;
    }

    public ISecuritySystemSettings AddExtensions(ISecuritySystemExtension extensions)
    {
        this.RegisterActions.Add(extensions.AddServices);

        return this;
    }

    public ISecuritySystemSettings SetAccessDeniedExceptionService<TAccessDeniedExceptionService>()
        where TAccessDeniedExceptionService : class, IAccessDeniedExceptionService
    {
        this.AccessDeniedExceptionServiceType = typeof(TAccessDeniedExceptionService);

        return this;
    }

    public ISecuritySystemSettings SetUserSource<TUserDomainObject>(
        Expression<Func<TUserDomainObject, Guid>> idPath,
        Expression<Func<TUserDomainObject, string>> namePath,
        Expression<Func<TUserDomainObject, bool>> filter)
    {
        this.RegisterUserSourceAction = sc =>
                                        {
                                            var info = new UserPathInfo<TUserDomainObject>(idPath, namePath, filter);
                                            sc.AddSingleton(info);
                                            sc.AddSingleton<IUserPathInfo>(info);

                                            sc.AddScoped<IUserSource<TUserDomainObject>, UserSource<TUserDomainObject>>();

                                            sc.AddScoped<ICurrentUserSource<TUserDomainObject>, CurrentUserSource<TUserDomainObject>>();
                                            sc.AddScopedFrom<ICurrentUserSource, ICurrentUserSource<TUserDomainObject>>();

                                            sc.AddScoped<IUserIdentitySource, UserIdentitySource<TUserDomainObject>>();
                                        };

        return this;
    }

    public ISecuritySystemSettings SetRunAsManager<TRunAsManager>()
        where TRunAsManager : class, IRunAsManager
    {
        this.RegisterRunAsManagerAction = sc => sc.AddScoped<IRunAsManager, TRunAsManager>();

        return this;
    }

    public ISecuritySystemSettings SetSecurityAccessorInfinityStorage<TStorage>()
        where TStorage : class, ISecurityAccessorInfinityStorage
    {
        this.SecurityAccessorInfinityStorageType = typeof(TStorage);

        return this;
    }

    public ISecuritySystemSettings SetDefaultSecurityRuleCredential(SecurityRuleCredential securityRuleCredential)
    {

        return this;
    }

    private void AddSecurityRole(IServiceCollection serviceCollection, FullSecurityRole fullSecurityRole)
    {
        if (this.InitializeAdministratorRole)
        {
            serviceCollection.AddSingleton(new PreInitializerFullSecurityRole(fullSecurityRole));
        }
        else
        {
            serviceCollection.AddSingleton(fullSecurityRole);
        }
    }
}
