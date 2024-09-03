using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalSource(this IServiceCollection sc) =>
        sc.AddScoped<ISecurityEntitySource, SecurityEntitySource>()
          .AddScoped(typeof(LocalStorage<>));
}
