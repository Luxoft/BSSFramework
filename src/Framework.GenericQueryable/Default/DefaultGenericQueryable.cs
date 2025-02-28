using System.Collections;
using System.Linq.Expressions;

namespace Framework.GenericQueryable.Default;

public class DefaultGenericQueryable<T>(IQueryable<T> baseSource) : IOrderedQueryable<T>
{
    public Type ElementType => baseSource.ElementType;

    public Expression Expression => baseSource.Expression;

    public IQueryProvider Provider { get; } = new DefaultGenericQueryProvider<T>(baseSource);

    public IEnumerator<T> GetEnumerator() => baseSource.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => baseSource.GetEnumerator();
}
