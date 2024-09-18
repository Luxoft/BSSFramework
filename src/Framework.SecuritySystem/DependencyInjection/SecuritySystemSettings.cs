﻿using System.Linq.Expressions;

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
    private readonly List<Action<IServiceCollection>> registerActions = new();

    private Action<IServiceCollection> registerUserSourceAction = _ => { };

    private Action<IServiceCollection> registerRunAsManagerAction= _ => { };

    private SecurityRuleCredential defaultSecurityRuleCredential = SecurityRuleCredential.CurrentUserWithRunAs;

    private Type accessDeniedExceptionServiceType = typeof(AccessDeniedExceptionService);

    private Type? securityAccessorInfinityStorageType;

    public bool InitializeAdministratorRole { get; set; } = true;

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
        this.registerActions.Add(sc => sc.RegisterSecurityContextSource(setup));

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
        this.registerActions.Add(sc => sc.AddSingleton(new SecurityRuleFullInfo(header, implementation)));

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

    public ISecuritySystemSettings SetUserSource<TUserDomainObject>(
        Expression<Func<TUserDomainObject, Guid>> idPath,
        Expression<Func<TUserDomainObject, string>> namePath,
        Expression<Func<TUserDomainObject, bool>> filter)
    {
        this.registerUserSourceAction = sc =>
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

    public void Initialize(IServiceCollection services)
    {
        this.registerActions.ForEach(v => v(services));

        this.registerUserSourceAction(services);
        this.registerRunAsManagerAction(services);

        if (this.InitializeAdministratorRole)
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
