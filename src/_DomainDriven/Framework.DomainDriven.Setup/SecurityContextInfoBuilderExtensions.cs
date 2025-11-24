using System.Linq.Expressions;

using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using SecuritySystem.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class SecurityContextInfoBuilderExtensions
{
    public static ISecurityContextInfoBuilder<TSecurityContext> SetDeepLevel<TSecurityContext>(
        this ISecurityContextInfoBuilder<TSecurityContext> builder,
        Expression<Func<TSecurityContext, int>> path)
    {
        return builder.AddExtension(services => services.Replace(ServiceDescriptor.Singleton(new DeepLevelInfo<TSecurityContext>(path))));
    }
}
