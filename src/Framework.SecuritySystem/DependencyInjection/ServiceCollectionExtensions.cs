using Framework.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGeneralSecuritySystem(this IServiceCollection services)
    {
        return services.AddSingleton<SecurityModeExpander>()
                       .AddSingleton<SecurityOperationExpander>()
                       .AddSingleton<SecurityRoleExpander>()

                       .AddSingleton<ISecurityRuleExpander, SecurityRuleExpander>()
                       .AddSingleton<ISecurityRoleSource, SecurityRoleSource>()

                       .AddSingleton<ISecurityContextInfoService, SecurityContextInfoService>()

                       .AddScoped<ISecurityPathProviderFactory, SecurityPathProviderFactory>()

                       .AddSingleton<ISecurityPathRestrictionService, SecurityPathRestrictionService>()

                       .AddScoped<ISecurityExpressionBuilderFactory, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.
                           SecurityExpressionBuilderFactory<Guid>>()

                       .AddSingleton<IAccessDeniedExceptionService, AccessDeniedExceptionService<Guid>>()

                       .AddSingleton(typeof(ISecurityProvider<>), typeof(DisabledSecurityProvider<>));
    }

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

    public static IServiceCollection AddSecuritySystem(this IServiceCollection services, Action<ISecuritySystemSettings> setup)
    {
        var settings = new SecuritySystemSettings();

        setup(settings);

        settings.RegisterActions.ForEach(v => v(services));

        if (settings.InitializeAdministratorRole)
        {
            services.AddSingleton<InitializedSecurityRoleSource>();
            services.AddSingletonFrom((InitializedSecurityRoleSource source) => source.GetSecurityRoles());
        }

        services.RegisterGeneralSecuritySystem();

        return services;
    }
}
