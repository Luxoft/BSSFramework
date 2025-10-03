using System.Collections.ObjectModel;
using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;

using Framework.Core;

namespace Framework.OData;

public class SelectOperation<TDomainObject> : IDynamicSelectOperation, IQueryableProcessor<TDomainObject>
{
    private readonly Lazy<bool> isVirtualLazy;

    private readonly Lazy<bool> isVirtualFilterLazy;

    private readonly Lazy<bool> isVirtualOrderLazy;

    public SelectOperation(Expression<Func<TDomainObject, bool>> filter, IEnumerable<ISelectOrder<TDomainObject>> orders, IEnumerable<Framework.QueryLanguage.LambdaExpression> expands, IEnumerable<Framework.QueryLanguage.LambdaExpression> selects, int skipCount, int takeCount)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));
        if (orders == null) throw new ArgumentNullException(nameof(orders));
        if (expands == null) throw new ArgumentNullException(nameof(expands));
        if (selects == null) throw new ArgumentNullException(nameof(selects));

        this.Filter = filter;
        this.Orders = orders.ToReadOnlyCollection();
        this.Expands = expands.ToReadOnlyCollection();
        this.Selects = selects.ToReadOnlyCollection();
        this.SkipCount = skipCount;
        this.TakeCount = takeCount;

        this.isVirtualFilterLazy = LazyHelper.Create(() => this.Filter.HasVirtualProperty());

        this.isVirtualOrderLazy = LazyHelper.Create(() => this.Orders.Any(order => order.HasVirtualProperty()));

        this.isVirtualLazy = LazyHelper.Create(() => this.isVirtualFilterLazy.Value || this.isVirtualOrderLazy.Value);
    }


    public Expression<Func<TDomainObject, bool>> Filter { get; private set; }

    public ReadOnlyCollection<ISelectOrder<TDomainObject>> Orders { get; private set; }

    public int SkipCount { get; private set; }

    public int TakeCount { get; private set; }


    public bool IsVirtual => this.isVirtualLazy.Value;

    public bool HasPaging => this.SkipCount != Default.SkipCount || this.TakeCount != Default.TakeCount;

    public SelectOperation<TDomainObject> WithoutPaging()
    {
        return new SelectOperation<TDomainObject>(
                                                  this.Filter,
                                                  this.Orders,
                                                  this.Expands,
                                                  this.Selects,
                                                  Default.SkipCount,
                                                  Default.TakeCount);
    }

    private ReadOnlyCollection<Framework.QueryLanguage.LambdaExpression> Expands { get; set; }

    private ReadOnlyCollection<Framework.QueryLanguage.LambdaExpression> Selects { get; set; }

    public SelectOperation<TDomainObject> AddFilter(Expression<Func<TDomainObject, bool>> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return this.OverrideFilter(this.Filter.BuildAnd(filter));
    }

    public SelectOperation<TDomainObject> OverrideFilter(Expression<Func<TDomainObject, bool>> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new SelectOperation<TDomainObject>(filter, this.Orders, this.Expands, this.Selects, this.SkipCount, this.TakeCount);
    }

    public SelectOperation<TDomainObject> AddOrder<TOrderKey>(Expression<Func<TDomainObject, TOrderKey>> order, OrderType type)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        return new SelectOperation<TDomainObject>(this.Filter, this.Orders.Concat(new[] { new SelectOrder<TDomainObject, TOrderKey>(order, type) }), this.Expands, this.Selects, this.SkipCount, this.TakeCount);
    }

    public SelectOperation<TOutput> Covariance<TOutput>()
            where TOutput : TDomainObject
    {
        var newFilter = this.Filter.Covariance<TOutput, TDomainObject, bool>();

        var newOrders = this.Orders.Select(order => order.Covariance<TOutput>());

        return new SelectOperation<TOutput>(newFilter, newOrders, this.Expands, this.Selects, this.SkipCount, this.TakeCount);
    }

    public SelectOperation<TDomainObject> ToCountOperation()
    {
        return new SelectOperation<TDomainObject>(this.Filter, Default.Orders, Default.Expands, Default.Selects, Default.SkipCount, Default.TakeCount);
    }

    public SelectOperation<TDomainObject> Visit(ExpressionVisitor visitor)
    {
        if (visitor == null) throw new ArgumentNullException(nameof(visitor));

        var newFilter = this.Filter.UpdateBody(visitor);

        var newOrders = this.Orders.Select(order => order.Visit(visitor));

        return new SelectOperation<TDomainObject>(newFilter, newOrders, this.Expands, this.Selects, this.SkipCount, this.TakeCount);
    }


    public IQueryable<TDomainObject> Process(IQueryable<TDomainObject> baseQueryable)
    {
        if (baseQueryable == null) throw new ArgumentNullException(nameof(baseQueryable));

        return this.GetProcessQueryableElements().Aggregate(baseQueryable);
    }

    private IEnumerable<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> GetProcessQueryableElements()
    {
        if (this.isVirtualFilterLazy.Value)
        {
            yield return q => q.Where(this.Filter.ToRealFilter())
                               .AsEnumerable()
                               .AsQueryable()
                               .Where(LambdaCompileCache.GetFunc(this.Filter))
                               .AsQueryable();
        }
        else
        {
            yield return q => q.Where(this.Filter);
        }

        yield return q => this.Orders.Aggregate(q, (query, order) => order.Process(query, this.isVirtualOrderLazy.Value));

        if (this.SkipCount != SelectOperation.Default.SkipCount)
        {
            yield return q => q.Skip(this.SkipCount);
        }

        if (this.TakeCount != SelectOperation.Default.TakeCount)
        {
            yield return q => q.Take(this.TakeCount);
        }
    }


    public static readonly SelectOperation<TDomainObject> Default = new SelectOperation<TDomainObject>(

     _ => true,

     new ISelectOrder<TDomainObject>[0],

     new Framework.QueryLanguage.LambdaExpression[0],

     new Framework.QueryLanguage.LambdaExpression[0],

     0,

     int.MaxValue);

    #region IDynamicSelectOperation Members

    IEnumerable<Framework.QueryLanguage.LambdaExpression> IDynamicSelectOperation.Expands
    {
        get { return this.Expands; }
    }

    IEnumerable<Framework.QueryLanguage.LambdaExpression> IDynamicSelectOperation.Selects
    {
        get { return this.Selects; }
    }

    #endregion



    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
