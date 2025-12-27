using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using Framework.Persistent;

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
        return builder.AddExtension(services => services.ReplaceSingleton(new DeepLevelInfo<TDomainObject>(path)));
    }
}
