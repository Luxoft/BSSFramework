using System.Collections.Immutable;
using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;

namespace Framework.OData;

public record SelectOperation<TDomainObject>(
    Expression<Func<TDomainObject, bool>> Filter,
    ImmutableArray<SelectOrder<TDomainObject>> Orders,
    int SkipCount,
    int TakeCount) : IDynamicSelectOperation, IQueryableProcessor<TDomainObject>
{
    public bool HasPaging => this.SkipCount != 0 || this.TakeCount != 0;

    public ImmutableArray<Framework.QueryLanguage.LambdaExpression> Expands { get; init; } = [];

    public ImmutableArray<Framework.QueryLanguage.LambdaExpression> Selects { get; init; } = [];

    public SelectOperation<TDomainObject> WithoutPaging() => this.HasPaging ? this with { SkipCount = 0, TakeCount = 0 } : this;

    public SelectOperation<TDomainObject> AddFilter(Expression<Func<TDomainObject, bool>> filter) =>

        this with { Filter = this.Filter.BuildAnd(filter) };

    public SelectOperation<TDomainObject> AddOrder<TOrderKey>(Expression<Func<TDomainObject, TOrderKey>> path, OrderType type) =>

        this with { Orders = [..this.Orders, new SelectOrder<TDomainObject, TOrderKey>(path) { OrderType = type }] };

    public SelectOperation<TDomainObject> ToCountOperation() => Default with { Filter = this.Filter };

    public SelectOperation<TDomainObject> Visit(ExpressionVisitor visitor)
    {
        var newFilter = this.Filter.UpdateBody(visitor);

        var newOrders = this.Orders.Select(order => order.Visit(visitor));

        return this with { Filter = newFilter, Orders = [.. newOrders] };
    }

    public IQueryable<TDomainObject> Process(IQueryable<TDomainObject> baseQueryable) =>
        this.GetProcessQueryableElements().Aggregate(baseQueryable);

    private IEnumerable<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> GetProcessQueryableElements()
    {
        yield return q => q.Where(this.Filter);

        yield return q => this.Orders.Aggregate(q, (query, order) => order.Process(query));

        if (this.SkipCount != SelectOperation.Default.SkipCount)
        {
            yield return q => q.Skip(this.SkipCount);
        }

        if (this.TakeCount != SelectOperation.Default.TakeCount)
        {
            yield return q => q.Take(this.TakeCount);
        }
    }


    public static readonly SelectOperation<TDomainObject> Default = new(_ => true, [], 0, int.MaxValue);

    IEnumerable<Framework.QueryLanguage.LambdaExpression> IDynamicSelectOperation.Expands => this.Expands;

    IEnumerable<Framework.QueryLanguage.LambdaExpression> IDynamicSelectOperation.Selects => this.Selects;
}
