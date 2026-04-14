using System.Linq.Expressions;

using CommonFramework;

namespace Framework.Database.Visitors;

public interface IOverrideEqualsDomainObjectMapper
{
    Maybe<BinaryExpression> TryReplace(BinaryExpression node);
}

