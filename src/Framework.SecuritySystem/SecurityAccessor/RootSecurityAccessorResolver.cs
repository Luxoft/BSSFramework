using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.SecurityAccessor;

public class RootSecurityAccessorResolver(
    ISecurityAccessorDataOptimizer optimizer,
    [FromKeyedServices(RawSecurityAccessorResolver.Key)]
    ISecurityAccessorResolver rawSecurityAccessorResolver) : ISecurityAccessorResolver
{
    public IEnumerable<string> Resolve(SecurityAccessorData data)
    {
        return rawSecurityAccessorResolver.Resolve(optimizer.Optimize(data));
    }
}
