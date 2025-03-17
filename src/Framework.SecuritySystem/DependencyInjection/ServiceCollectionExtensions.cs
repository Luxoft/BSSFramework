using System.Linq.Expressions;

using Framework.DependencyInjection;
using Framework.SecuritySystem.AvailableSecurity;
using Framework.SecuritySystem.Builders._Factory;

using Framework.SecuritySystem.Builders.AccessorsBuilder;
using Framework.SecuritySystem.Builders.MaterializedBuilder;
using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.ExternalSystem.ApplicationSecurity;
using Framework.SecuritySystem.ExternalSystem.Management;
using Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;
using Framework.SecuritySystem.PermissionOptimization;
using Framework.SecuritySystem.SecurityAccessor;
using Framework.SecuritySystem.SecurityRuleInfo;
using Framework.SecuritySystem.Services;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDomainSecurityServices(
        this IServiceCollection services,
        Action<IDomainSecurityServiceRootBuilder> setupAction)
    {
        var builder = new DomainSecurityServiceRootBuilder();

        setupAction(builder);

        builder.Register(services);

        return services;
    }

    public static IServiceCollection RegisterSecurityContextInfoSource(
        this IServiceCollection services,
        Action<ISecurityContextInfoBuilder> setup)
    {
        var builder = new SecurityContextInfoBuilder();

        setup(builder);

        builder.Register(services);

        return services;
    }

    public static IServiceCollection AddSecuritySystem(this IServiceCollection services, Action<ISecuritySystemSettings> setupAction)
    {
        services.RegisterGeneralSecuritySystem();

        var settings = new SecuritySystemSettings();

        setupAction(settings);

        settings.Initialize(services);

        return services;
    }

    public static IServiceCollection AddRelativeDomainPath<TFrom, TTo>(
        this IServiceCollection services,
        Expression<Func<TFrom, TTo>> path,
        string? key = null)
    {
        var info = new RelativeDomainPathInfo<TFrom, TTo>(path);

        if (key == null)
        {
            return services.AddSingleton<IRelativeDomainPathInfo<TFrom, TTo>>(info);
        }
        else
        {
            return services.AddKeyedSingleton<IRelativeDomainPathInfo<TFrom, TTo>>(key, info);
        }
    }

    private static IServiceCollection RegisterGeneralSecuritySystem(this IServiceCollection services)
    {
        return services.AddSingleton<SecurityAdministratorRuleFactory>()

                       .AddScoped<ISecurityContextStorage, SecurityContextStorage>()
                       .AddScoped(typeof(LocalStorage<>))

                       .AddScoped<IUserCredentialNameResolver, RootUserCredentialNameResolver>()

                       .AddScoped<IRootPrincipalSourceService, RootPrincipalSourceService>()
                       .AddNotImplemented<IPrincipalManagementService>(
                           $"{nameof(IPrincipalManagementService)} not supported",
                           isScoped: true)

                       .AddSingleton<IClientSecurityRuleNameExtractor, ClientSecurityRuleNameExtractor>()
                       .AddSingleton<IClientSecurityRuleInfoSource, RootClientSecurityRuleInfoSource>()
                       .AddKeyedSingleton<IClientSecurityRuleInfoSource, DomainModeClientSecurityRuleInfoSource>(
                           RootClientSecurityRuleInfoSource.ElementKey)
                       .AddSingleton<IClientSecurityRuleResolver, ClientSecurityRuleResolver>()
                       .AddSingleton<IDomainModeSecurityRuleResolver, DomainModeSecurityRuleResolver>()
                       .AddSingleton<IDomainSecurityRoleExtractor, DomainSecurityRoleExtractor>()

                       .AddSingleton<ISecurityRuleHeaderExpander, SecurityRuleHeaderExpander>()
                       .AddSingleton<IClientSecurityRuleExpander, ClientSecurityRuleExpander>()
                       .AddSingleton<ISecurityModeExpander, SecurityModeExpander>()
                       .AddSingleton<ISecurityOperationExpander, SecurityOperationExpander>()
                       .AddSingleton<ISecurityRoleExpander, SecurityRoleExpander>()
                       .AddSingleton<IRoleFactorySecurityRuleExpander, RoleFactorySecurityRuleExpander>()
                       .AddSingleton<ISecurityRuleExpander, RootSecurityRuleExpander>()
                       .AddSingleton<ISecurityRoleSource, SecurityRoleSource>()
                       .AddSingleton<ISecurityOperationInfoSource, SecurityOperationInfoSource>()
                       .AddScoped<ISecurityContextSource, SecurityContextSource>()
                       .AddSingleton<ISecurityContextInfoSource, SecurityContextInfoSource>()
                       .AddSingleton<ISecurityRuleBasicOptimizer, SecurityRuleBasicOptimizer>()
                       .AddSingleton<ISecurityRuleDeepOptimizer, SecurityRuleDeepOptimizer>()

                       .AddScoped(typeof(IRoleBaseSecurityProviderFactory<>), typeof(RoleBaseSecurityProviderFactory<>))
                       .AddScoped(typeof(IDomainSecurityProviderFactory<>), typeof(DomainSecurityProviderFactory<>))
                       .AddSingleton<ISecurityPathRestrictionService, SecurityPathRestrictionService>()
                       .AddScoped(typeof(ISecurityFilterFactory<>), typeof(SecurityFilterBuilderFactory<>))
                       .AddScoped(typeof(IAccessorsFilterFactory<>), typeof(AccessorsFilterBuilderFactory<>))
                       .AddScoped<ICurrentUser, CurrentUser>()
                       .AddKeyedScoped(
                           typeof(ISecurityProvider<>),
                           nameof(DomainSecurityRule.CurrentUser),
                           typeof(CurrentUserSecurityProvider<>))
                       .AddKeyedSingleton(
                           typeof(ISecurityProvider<>),
                           nameof(DomainSecurityRule.AccessDenied),
                           typeof(AccessDeniedSecurityProvider<>))
                       .AddKeyedSingleton(typeof(ISecurityProvider<>), nameof(SecurityRule.Disabled), typeof(DisabledSecurityProvider<>))
                       .AddSingleton(typeof(ISecurityProvider<>), typeof(DisabledSecurityProvider<>))
                       .AddScoped(typeof(IDomainSecurityService<>), typeof(ContextDomainSecurityService<>))

                       .AddScoped<ISecuritySystemFactory, SecuritySystemFactory>()
                       .AddScoped(
                           sp =>
                           {
                               var factory = sp.GetRequiredService<ISecuritySystemFactory>();
                               var securityRuleCredential = sp.GetRequiredService<SecurityRuleCredential>();

                               return factory.Create(securityRuleCredential);
                           })

                       .AddKeyedScoped(
                           nameof(SecurityRuleCredential.CurrentUserWithoutRunAsCredential),
                           (sp, _) => sp.GetRequiredService<ISecuritySystemFactory>()
                                        .Create(new SecurityRuleCredential.CurrentUserWithoutRunAsCredential()))

                       .AddScoped(
                           sp =>
                           {
                               var factoryList = sp.GetRequiredService<IEnumerable<IPermissionSystemFactory>>();
                               var securityRuleCredential = sp.GetRequiredService<SecurityRuleCredential>();

                               return factoryList.Select(factory => factory.Create(securityRuleCredential));
                           })

                       .AddSingleton<ISecurityRolesIdentsResolver, SecurityRolesIdentsResolver>()

                       .AddSingleton<IRuntimePermissionOptimizationService, RuntimePermissionOptimizationService>()

                       .AddSingleton<ISecurityAccessorDataOptimizer, SecurityAccessorDataOptimizer>()
                       .AddKeyedScoped<ISecurityAccessorResolver, RawSecurityAccessorResolver>(RawSecurityAccessorResolver.Key)
                       .AddScoped<ISecurityAccessorResolver, RootSecurityAccessorResolver>()

                       .AddScoped<IAvailableSecurityRoleSource, AvailableSecurityRoleSource>()
                       .AddScoped<IAvailableSecurityOperationSource, AvailableSecurityOperationSource>()
                       .AddScoped<IAvailableClientSecurityRuleSource, AvailableClientSecurityRuleSource>()
                       .AddScoped<IUserNameResolver, UserNameResolver>();
    }
}
