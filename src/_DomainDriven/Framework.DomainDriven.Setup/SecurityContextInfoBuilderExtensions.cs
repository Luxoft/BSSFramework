using System.Linq.Expressions;

using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using SecuritySystem;
using SecuritySystem.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class SecurityContextInfoBuilderExtensions
{
    public static ISecurityContextInfoBuilder<TDomainObject> SetDeepLevel<TDomainObject>(
        this ISecurityContextInfoBuilder<TDomainObject> builder,
        Expression<Func<TDomainObject, int>> path)
        where TDomainObject : class, ISecurityContext
    {
        return builder.AddExtension(services => services.Replace(ServiceDescriptor.Singleton(new DeepLevelInfo<TDomainObject>(path))));
    }
}
