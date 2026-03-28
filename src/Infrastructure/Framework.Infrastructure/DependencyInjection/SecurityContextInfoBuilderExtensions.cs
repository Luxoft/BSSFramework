using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using HierarchicalExpand;

using SecuritySystem;
using SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class SecurityContextInfoBuilderExtensions
{
    public static ISecurityContextInfoBuilder<TDomainObject> SetDeepLevel<TDomainObject>(
        this ISecurityContextInfoBuilder<TDomainObject> builder,
        Expression<Func<TDomainObject, int>> path)
        where TDomainObject : class, ISecurityContext =>
        builder.AddExtension(services => services.ReplaceSingleton(new DeepLevelInfo<TDomainObject>(path)));
}
