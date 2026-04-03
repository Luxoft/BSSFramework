using System.Linq.Expressions;

using CommonFramework;

namespace Framework.Database._Visitors;

public interface IOverrideEqualsDomainObjectMapper
{
    Maybe<BinaryExpression> TryReplace(BinaryExpression node);
}

