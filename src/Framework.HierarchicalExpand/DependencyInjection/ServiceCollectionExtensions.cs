using Microsoft.Extensions.DependencyInjection;

namespace Framework.HierarchicalExpand.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterHierarchicalObjectExpander(this IServiceCollection services)
    {
        return services.AddSingleton<IRealTypeResolver, IdentityRealTypeResolver>()
                       .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<Guid>>();
    }
}
