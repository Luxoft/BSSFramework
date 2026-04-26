using System.Linq.Expressions;

using Anch.Core;
using Anch.IdentitySource;

namespace Framework.Database.Visitors.Containers;

public class OverrideEqualsDomainObjectVisitorContainer(IServiceProxyFactory serviceProxyFactory, IIdentityInfoSource identityInfoSource) : IExpressionVisitorContainer
{
    public ExpressionVisitor Visitor { get; } = new OverrideEqualsDomainObjectVisitor(serviceProxyFactory, identityInfoSource);
}
