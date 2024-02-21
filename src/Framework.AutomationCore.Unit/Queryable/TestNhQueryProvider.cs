using System.Linq.Expressions;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Type;

namespace Framework.AutomationCore.Unit.Queryable;

internal class TestNhQueryProvider<TDomainObject> : INhQueryProvider
{
    public TestNhQueryProvider(IQueryable<TDomainObject> source) => this.Source = source;

    private IQueryable<TDomainObject> Source { get; }

    public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
        new TestQueryable<TElement>(this.Source.Provider.CreateQuery<TElement>(expression));

    public object Execute(Expression expression) => this.ExecuteInMemoryQuery(expression);

    public TResult Execute<TResult>(Expression expression) => this.ExecuteInMemoryQuery<TResult>(expression);

    public async Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) =>
        this.Execute<TResult>(expression);

    public int ExecuteDml<T1>(QueryMode queryMode, Expression expression) => throw new NotImplementedException();

    public async Task<int> ExecuteDmlAsync<T1>(QueryMode queryMode, Expression expression, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public IFutureEnumerable<TResult> ExecuteFuture<TResult>(Expression expression) => throw new NotImplementedException();

    public IFutureValue<TResult> ExecuteFutureValue<TResult>(Expression expression) => throw new NotImplementedException();

    public void SetResultTransformerAndAdditionalCriteria(
        IQuery query,
        NhLinqExpression nhExpression,
        IDictionary<string, Tuple<object, IType>> parameters) =>
        throw new NotImplementedException();

    private object ExecuteInMemoryQuery(Expression expression) =>
        this.Source.Provider.Execute(new ExpressionTreeModifier().Visit(expression)) ?? throw new Exception();

    private TResult ExecuteInMemoryQuery<TResult>(Expression expression) =>
        this.Source.Provider.Execute<TResult>(new ExpressionTreeModifier().Visit(expression));
}
