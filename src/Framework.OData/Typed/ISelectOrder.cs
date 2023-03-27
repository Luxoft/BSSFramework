using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.OData;

public interface ISelectOrder<TDomainObject>
{
    LambdaExpression Path { get; }

    OrderType OrderType { get; }


    IQueryable<TDomainObject> Process([NotNull] IQueryable<TDomainObject> queryable, bool compile);

    ISelectOrder<TOutput> Covariance<TOutput>()
            where TOutput : TDomainObject;

    ISelectOrder<TDomainObject> Visit([NotNull] ExpressionVisitor visitor);
}
