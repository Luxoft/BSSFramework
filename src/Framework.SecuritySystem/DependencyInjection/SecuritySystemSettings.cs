using System.Linq.Expressions;

using Framework.DependencyInjection;
using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.Services;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public class SecuritySystemSettings : ISecuritySystemSettings
{
    public List<Action<IServiceCollection>> RegisterActions { get; set; } = new();

    public bool InitializeAdministratorRole { get;  set; } = true;

    public Type AccessDeniedExceptionServiceType { get; private set; } = typeof(AccessDeniedExceptionService<Guid>);

    public Type CurrentUserType { get; private set; } = typeof(CurrentUser);

    public ISecuritySystemSettings AddSecurityContext<TSecurityContext>(
        Guid ident,
        string? name,
        Func<TSecurityContext, string>? displayFunc)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        return this.AddSecurityContext(b => b.Add(ident, name, displayFunc));
    }

    public ISecuritySystemSettings AddSecurityContext(Action<ISecurityContextInfoBuilder<Guid>> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterSecurityContextInfoService(setup));

        return this;
    }

    public ISecuritySystemSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterDomainSecurityServices<Guid>(setup));

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

    public ISecuritySystemSettings SetCurrentUser<TCurrentUser>()
        where TCurrentUser : ICurrentUser
    {
        this.CurrentUserType = typeof(TCurrentUser);

        return this;
    }

    public ISecuritySystemSettings SetUserSource<TUserDomainObject>(
        Expression<Func<TUserDomainObject, Guid>> idPath,
        Expression<Func<TUserDomainObject, string>> namePath,
        Expression<Func<TUserDomainObject, bool>> filter)
    {
        this.RegisterActions.Add(
            sc =>
            {
                var info = new UserPathInfo<TUserDomainObject>(idPath, namePath, filter);
                sc.AddSingleton(info);
                sc.AddSingleton<IUserPathInfo>(info);

                sc.AddScoped<IUserSource<TUserDomainObject>, UserSource<TUserDomainObject>>();

                sc.AddScoped<ICurrentUserSource<TUserDomainObject>, CurrentUserSource<TUserDomainObject>>();
                sc.AddScopedFrom<ICurrentUserSource, ICurrentUserSource<TUserDomainObject>>();

                sc.AddScoped(typeof(CurrentUserSecurityProvider<>)); // can't define partial generics
                sc.AddScoped(typeof(CurrentUserSecurityProvider<,>));

                sc.AddScoped<IUserIdentitySource, UserIdentitySource<TUserDomainObject>>();
            });

        this.SetCurrentUserSecurityProvider(typeof(CurrentUserSecurityProvider<>));

        return this;
    }

    public ISecuritySystemSettings SetCurrentUserSecurityProvider(Type genericSecurityProviderType)
    {
        this.RegisterActions.Add(
            sc => sc.AddKeyedScoped(typeof(ISecurityProvider<>), nameof(DomainSecurityRule.CurrentUser), genericSecurityProviderType));

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
