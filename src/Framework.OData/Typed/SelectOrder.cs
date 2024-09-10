using System.Linq.Expressions;

using Framework.Core;

namespace Framework.OData;

public class SelectOrder<TDomainObject, TOrderKey> : ISelectOrder<TDomainObject>
{
    public SelectOrder(Expression<Func<TDomainObject, TOrderKey>> path, OrderType orderType)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (!Enum.IsDefined(typeof (OrderType), orderType)) throw new ArgumentOutOfRangeException(nameof(orderType));

        this.Path = path;
        this.OrderType = orderType;
    }


    public Expression<Func<TDomainObject, TOrderKey>> Path { get; private set; }
    public OrderType OrderType { get; private set; }


    public IQueryable<TDomainObject> Process(IQueryable<TDomainObject> queryable, bool compile)
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));

        if (compile)
        {
            var path = this.Path.Compile(LambdaCompileCache);

            switch (this.OrderType)
            {
                case OrderType.Asc:
                    return queryable.OrderBy(path).AsQueryable();

                case OrderType.Desc:
                    return queryable.OrderByDescending(path).AsQueryable();

                default:
                    throw new ArgumentOutOfRangeException("this._orderType");
            }
        }
        else
        {
            switch (this.OrderType)
            {
                case OrderType.Asc:
                    return queryable.OrderBy(this.Path);

                case OrderType.Desc:
                    return queryable.OrderByDescending(this.Path);

                default:
                    throw new ArgumentOutOfRangeException("this._orderType");
            }
        }
    }

    public ISelectOrder<TOutput> Covariance<TOutput>()
            where TOutput : TDomainObject
    {
        var newPath = this.Path.Covariance<TOutput, TDomainObject, TOrderKey>();

        return new SelectOrder<TOutput, TOrderKey>(newPath, this.OrderType);
    }

    public ISelectOrder<TDomainObject> Visit(ExpressionVisitor visitor)
    {
        if (visitor == null) throw new ArgumentNullException(nameof(visitor));

        var newPath = this.Path.UpdateBody(visitor);

        return new SelectOrder<TDomainObject, TOrderKey>(newPath, this.OrderType);
    }


    LambdaExpression ISelectOrder<TDomainObject>.Path => this.Path;


    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
