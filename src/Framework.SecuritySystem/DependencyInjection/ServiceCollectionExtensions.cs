using System.Linq.Expressions;

using Framework.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.Rules.Builders;
using Framework.SecuritySystem.Rules.Builders.MaterializedPermissions;
using Framework.SecuritySystem.Services;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDomainSecurityServices<TIdent>(
        this IServiceCollection services,
        Action<IDomainSecurityServiceRootBuilder> setupAction)
    {
        var builder = new DomainSecurityServiceRootBuilder<TIdent>();

        setupAction(builder);

        builder.Register(services);

        return services;
    }

    public static IServiceCollection RegisterSecurityContextInfoService<TIdent>(
        this IServiceCollection services,
        Action<ISecurityContextInfoBuilder<TIdent>> setup)
    {
        var builder = new SecurityContextInfoBuilder<TIdent>();

        setup(builder);

        builder.Register(services);

        return services;
    }

    public static IServiceCollection AddSecuritySystem(this IServiceCollection services, Action<ISecuritySystemSettings> setupAction)
    {
        var settings = new SecuritySystemSettings();

        setupAction(settings);

        settings.RegisterActions.ForEach(v => v(services));
        settings.RegisterUserSourceAction(services);

        if (settings.InitializeAdministratorRole)
        {
            services.AddSingleton<IInitializedSecurityRoleSource, InitializedSecurityRoleSource>();
            services.AddSingletonFrom((IInitializedSecurityRoleSource source) => source.GetSecurityRoles());
        }

        services.AddSingleton(typeof(IAccessDeniedExceptionService), settings.AccessDeniedExceptionServiceType);

        services.AddScoped(typeof(ICurrentUser), settings.CurrentUserType);

        services.RegisterGeneralSecuritySystem();

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
        return services.AddSingleton<SecurityModeExpander>()
                       .AddSingleton<SecurityOperationExpander>()
                       .AddSingleton<SecurityRoleExpander>()
                       .AddSingleton<RoleFactorySecurityRuleExpander>()
                       .AddSingleton<ISecurityRuleExpander, SecurityRuleExpander>()
                       .AddSingleton<ISecurityRoleSource, SecurityRoleSource>()
                       .AddSingleton<ISecurityOperationInfoSource, SecurityOperationInfoSource>()
                       .AddSingletonFrom<ISecurityContextInfoService, ISecurityContextInfoService<Guid>>()
                       .AddSingleton<ISecurityContextInfoService<Guid>, SecurityContextInfoService<Guid>>()
                       .AddScoped<IDomainSecurityProviderFactory, DomainSecurityProviderFactory>()
                       .AddSingleton<ISecurityRuleBasicOptimizer, SecurityRuleBasicOptimizer>()
                       .AddSingleton<ISecurityRuleDeepOptimizer, SecurityRuleDeepOptimizer>()
                       .AddSingleton<ISecurityRuleImplementationResolver, SecurityRuleImplementationResolver>()
                       .AddScoped<IRoleBaseSecurityProviderFactory, RoleBaseSecurityProviderFactory>()
                       .AddSingleton<ISecurityPathRestrictionService, SecurityPathRestrictionService>()
                       .AddScoped<ISecurityExpressionBuilderFactory, SecurityExpressionBuilderFactory<Guid>>()
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
                       .AddScopedFrom<IAuthorizationSystem, IAuthorizationSystem<Guid>>()
                       .AddScopedFrom<IOperationAccessor, IAuthorizationSystem>();
    }
}
