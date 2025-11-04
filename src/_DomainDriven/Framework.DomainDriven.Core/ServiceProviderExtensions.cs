using SecuritySystem.HierarchicalExpand;

namespace Framework.DomainDriven;

public static class ServiceProviderExtensions
{
    public static bool IsHierarchical(this IServiceProvider serviceProvider, Type type)
    {
        return serviceProvider.GetService(typeof(HierarchicalInfo<>).MakeGenericType(type)) != null;
    }
}
