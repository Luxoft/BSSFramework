namespace Framework.Authorization.SecuritySystem;

public interface IRuntimePermissionOptimizationService
{
    IEnumerable<Dictionary<Type, List<Guid>>> Optimize(IEnumerable<Dictionary<Type, List<Guid>>> permissions);
}
