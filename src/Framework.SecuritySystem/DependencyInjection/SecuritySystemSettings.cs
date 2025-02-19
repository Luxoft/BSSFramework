using System.Linq.Expressions;

using Framework.DependencyInjection;
using Framework.DomainDriven.ApplicationSecurity;
using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.SecurityAccessor;
using Framework.SecuritySystem.SecurityRuleInfo;
using Framework.SecuritySystem.Services;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public class SecuritySystemSettings : ISecuritySystemSettings
{
    private DomainSecurityRule.RoleBaseSecurityRule securityAdministratorRule = SecurityRole.Administrator;

    private readonly List<Action<IServiceCollection>> registerActions = [];

    private Action<IServiceCollection> registerUserSourceAction = _ => { };

    private Action<IServiceCollection> registerRunAsManagerAction= _ => { };

    private SecurityRuleCredential defaultSecurityRuleCredential = new SecurityRuleCredential.CurrentUserWithRunAsCredential();

    private Type accessDeniedExceptionServiceType = typeof(AccessDeniedExceptionService);

    private Type clientDomainModeSecurityRuleSource = typeof(ClientDomainModeSecurityRuleSource);

    private Type? securityAccessorInfinityStorageType;

    public bool InitializeDefaultRoles { get; set; } = true;

    public ISecuritySystemSettings SetSecurityAdministratorRule(DomainSecurityRule.RoleBaseSecurityRule rule)
    {
        this.securityAdministratorRule = rule;

        return this;
    }

    public ISecuritySystemSettings AddSecurityContext<TSecurityContext>(
        Guid ident,
        string? name,
        Func<TSecurityContext, string>? displayFunc)
        where TSecurityContext : ISecurityContext
    {
        return this.AddSecurityContext(b => b.Add(ident, name, displayFunc));
    }

    public ISecuritySystemSettings AddSecurityContext(Action<ISecurityContextInfoBuilder> setup)
    {
        this.registerActions.Add(sc => sc.RegisterSecurityContextInfoSource(setup));

        return this;
    }

    public ISecuritySystemSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup)
    {
        this.registerActions.Add(sc => sc.RegisterDomainSecurityServices(setup));

        return this;
    }

    public ISecuritySystemSettings AddSecurityRole(SecurityRole securityRole, SecurityRoleInfo info)
    {
        this.registerActions.Add(sc => this.AddSecurityRole(sc, new FullSecurityRole(securityRole.Name, info)));

        return this;
    }

    public ISecuritySystemSettings AddSecurityRule(DomainSecurityRule.SecurityRuleHeader header, DomainSecurityRule implementation)
    {
        this.registerActions.Add(sc => sc.AddSingleton(new SecurityRuleHeaderInfo(header, implementation)));

        return this;
    }

    public ISecuritySystemSettings AddSecurityOperation(SecurityOperation securityOperation, SecurityOperationInfo info)
    {
        this.registerActions.Add(sc => sc.AddSingleton(new FullSecurityOperation(securityOperation, info)));

        return this;
    }

    public ISecuritySystemSettings AddPermissionSystem<TPermissionSystemFactory>()
        where TPermissionSystemFactory : class, IPermissionSystemFactory
    {
        this.registerActions.Add(sc => sc.AddScoped<IPermissionSystemFactory, TPermissionSystemFactory>());

        return this;
    }

    public ISecuritySystemSettings AddPermissionSystem(Func<IServiceProvider, IPermissionSystemFactory> getFactory)
    {
        this.registerActions.Add(sc => sc.AddScoped(getFactory));

        return this;
    }

    public ISecuritySystemSettings AddExtensions(ISecuritySystemExtension extensions)
    {
        this.registerActions.Add(extensions.AddServices);

        return this;
    }

    public ISecuritySystemSettings SetAccessDeniedExceptionService<TAccessDeniedExceptionService>()
        where TAccessDeniedExceptionService : class, IAccessDeniedExceptionService
    {
        this.accessDeniedExceptionServiceType = typeof(TAccessDeniedExceptionService);

        return this;
    }

    public ISecuritySystemSettings SetUserSource<TUser>(
        Expression<Func<TUser, Guid>> idPath,
        Expression<Func<TUser, string>> namePath,
        Expression<Func<TUser, bool>> filter,
        Expression<Func<TUser, TUser?>>? runAsPath = null)
    {
        this.registerUserSourceAction = sc =>
                                        {
                                            var info = new UserPathInfo<TUser>(idPath, namePath, filter);
                                            sc.AddSingleton(info);
                                            sc.AddSingleton<IUserPathInfo>(info);

                                            sc.AddScoped<IUserSource<TUser>, UserSource<TUser>>();
                                            sc.AddScopedFrom<IUserSource, IUserSource<TUser>>();

                                            sc.AddScoped<ICurrentUserSource<TUser>, CurrentUserSource<TUser>>();

                                            sc.AddScoped<IRunAsValidator, UserSourceRunAsValidator<TUser>>();

                                            sc.AddScoped<IUserCredentialNameByIdResolver, UserSourceCredentialNameByIdResolver<TUser>>();

                                            if (runAsPath != null)
                                            {
                                                sc.AddSingleton(new UserSourceRunAsAccessorData<TUser>(runAsPath));
                                                sc.AddSingleton<IUserSourceRunAsAccessor<TUser>, UserSourceRunAsAccessor<TUser>>();
                                                sc.AddScoped<IRunAsManager, UserSourceRunAsManager<TUser>>();
                                            }
                                        };

        return this;
    }

    public ISecuritySystemSettings SetRunAsManager<TRunAsManager>()
        where TRunAsManager : class, IRunAsManager
    {
        this.registerRunAsManagerAction = sc => sc.AddScoped<IRunAsManager, TRunAsManager>();

        return this;
    }

    public ISecuritySystemSettings SetSecurityAccessorInfinityStorage<TStorage>()
        where TStorage : class, ISecurityAccessorInfinityStorage
    {
        this.securityAccessorInfinityStorageType = typeof(TStorage);

        return this;
    }

    public ISecuritySystemSettings SetDefaultSecurityRuleCredential(SecurityRuleCredential securityRuleCredential)
    {
        this.defaultSecurityRuleCredential = securityRuleCredential;

        return this;
    }

    public ISecuritySystemSettings SetClientDomainModeSecurityRuleSource<TClientDomainModeSecurityRuleSource>()
        where TClientDomainModeSecurityRuleSource : class, IClientDomainModeSecurityRuleSource
    {
        this.clientDomainModeSecurityRuleSource = typeof(TClientDomainModeSecurityRuleSource);

        return this;
    }

    public ISecuritySystemSettings AddClientSecurityRuleInfoSource<TClientSecurityRuleInfoSource>()
        where TClientSecurityRuleInfoSource : class, IClientSecurityRuleInfoSource
    {
        this.registerActions.Add(sc => sc.AddKeyedSingleton<IClientSecurityRuleInfoSource, TClientSecurityRuleInfoSource>(RootClientSecurityRuleInfoSource.ElementKey));

        return this;
    }

    public ISecuritySystemSettings AddClientSecurityRuleInfoSource(Type sourceType)
    {
        this.registerActions.Add(
            sc => sc.AddKeyedSingleton<IClientSecurityRuleInfoSource>(RootClientSecurityRuleInfoSource.ElementKey,
                (sp, _) => ActivatorUtilities.CreateInstance<SourceTypeClientSecurityRuleInfoSource>(sp, sourceType)));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton(new SecurityAdministratorRuleInfo(this.securityAdministratorRule));

        this.registerActions.ForEach(v => v(services));

        this.registerUserSourceAction(services);
        this.registerRunAsManagerAction(services);

        if (this.InitializeDefaultRoles)
        {
            services.AddSingleton<IInitializedSecurityRoleSource, InitializedSecurityRoleSource>();
            services.AddSingletonFrom((IInitializedSecurityRoleSource source) => source.GetSecurityRoles());
        }

        services.AddSingleton(typeof(IAccessDeniedExceptionService), this.accessDeniedExceptionServiceType);

        if (this.securityAccessorInfinityStorageType == null)
        {
            services.AddNotImplemented<ISecurityAccessorInfinityStorage>(
                "Use 'SetSecurityAccessorInfinityStorage' for initialize infinity storage");
        }
        else
        {
            services.AddScoped(typeof(ISecurityAccessorInfinityStorage), this.securityAccessorInfinityStorageType);
        }

        services.AddSingleton(this.defaultSecurityRuleCredential);

        services.AddSingleton(typeof(IClientDomainModeSecurityRuleSource), this.clientDomainModeSecurityRuleSource);
    }

    private void AddSecurityRole(IServiceCollection serviceCollection, FullSecurityRole fullSecurityRole)
    {
        if (this.InitializeDefaultRoles)
        {
            serviceCollection.AddSingleton(new PreInitializerFullSecurityRole(fullSecurityRole));
        }
        else
        {
            serviceCollection.AddSingleton(fullSecurityRole);
        }
    }
}
