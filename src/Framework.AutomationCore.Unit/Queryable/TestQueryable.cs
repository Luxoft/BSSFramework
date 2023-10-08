using System.Collections;
using System.Linq.Expressions;

namespace Framework.AutomationCore.Unit.Queryable;

public class TestQueryable<TDomainObject> : IOrderedQueryable<TDomainObject>
{
    private readonly IQueryable<TDomainObject> queryable;

    public TestQueryable(IQueryable<TDomainObject> queryable)
    {
        this.queryable = queryable;
        this.Provider = new TestNhQueryProvider<TDomainObject>(this.queryable);
    }

    public TestQueryable(IEnumerable<TDomainObject> enumerable)
    {
        this.queryable = enumerable.AsQueryable();
        this.Provider = new TestNhQueryProvider<TDomainObject>(this.queryable);
    }

    public TestQueryable(params TDomainObject[] args)
    {
        this.queryable = args.AsQueryable();
        this.Provider = new TestNhQueryProvider<TDomainObject>(this.queryable);
    }

    public Expression Expression => this.queryable.Expression;

    IEnumerator IEnumerable.GetEnumerator() => this.queryable.GetEnumerator();

    public Type ElementType => this.queryable.ElementType;

    public IQueryProvider Provider { get; }

    public IEnumerator<TDomainObject> GetEnumerator() => this.queryable.GetEnumerator();
}
