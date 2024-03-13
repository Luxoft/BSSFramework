using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

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
}
