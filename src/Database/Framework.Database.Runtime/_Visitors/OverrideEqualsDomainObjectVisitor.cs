using System.Collections.Concurrent;
using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.IdentitySource;
using CommonFramework.Maybe;

namespace Framework.Database._Visitors;

public class OverrideEqualsDomainObjectVisitor(IServiceProxyFactory serviceProxyFactory, IIdentityInfoSource identityInfoSource) : ExpressionVisitor
{
    private readonly ConcurrentDictionary<Type, IOverrideEqualsDomainObjectMapper?> mapperCache = [];

    protected override Expression VisitBinary(BinaryExpression baseNode)
    {
        var baseVisited = base.VisitBinary(baseNode);

        var request = from node in (baseVisited as BinaryExpression).ToMaybe()

                      where node.Left.Type == node.Right.Type && node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual

                      from mapper in this.TryGetMapper(node.Left.Type).ToMaybe()

                      from res in mapper.TryReplace(node)

                      select res;

        return request.GetValueOrDefault(baseVisited);
    }

    private IOverrideEqualsDomainObjectMapper? TryGetMapper(Type domainType) =>

        this.mapperCache.GetOrAdd(
            domainType,
            _ =>
            {
                if (identityInfoSource.TryGetIdentityInfo(domainType) is { } identityInfo)
                {
                    var innerServiceType = typeof(OverrideEqualsDomainObjectMapper<,>).MakeGenericType(identityInfo.DomainObjectType, identityInfo.IdentityType);

                    var innerService = serviceProxyFactory.Create<IOverrideEqualsDomainObjectMapper>(innerServiceType, identityInfo);

                    return innerService;
                }
                else
                {
                    return null;
                }
            });
}
