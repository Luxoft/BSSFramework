using System.Linq.Expressions;

namespace Framework.OData;

public interface ISelectOrder<TDomainObject>
{
    LambdaExpression Path { get; }

    OrderType OrderType { get; }


    IQueryable<TDomainObject> Process(IQueryable<TDomainObject> queryable, bool compile);

    ISelectOrder<TOutput> Covariance<TOutput>()
            where TOutput : TDomainObject;

    ISelectOrder<TDomainObject> Visit(ExpressionVisitor visitor);
}
