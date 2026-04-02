using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.IdentitySource;

namespace Framework.Database._Visitors.Containers;

public class OverrideEqualsDomainObjectVisitorContainer(IServiceProxyFactory serviceProxyFactory, IIdentityInfoSource identityInfoSource) : IExpressionVisitorContainer
{
    public ExpressionVisitor Visitor { get; } = new OverrideEqualsDomainObjectVisitor(serviceProxyFactory, identityInfoSource);
}
