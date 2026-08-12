using System.Linq.Expressions;

using Anch.Core;

namespace Framework.Database.Visitors;

public interface IOverrideEqualsDomainObjectMapper
{
    Maybe<BinaryExpression> TryReplace(BinaryExpression node);
}

