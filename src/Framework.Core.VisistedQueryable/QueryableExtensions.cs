using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core;

public static class VisitQueryableExtensions
{
    public static IQueryable<T> Visit<T>(this IQueryable<T> queryable, ExpressionVisitor visitor)
    {
        return new VisitedQueryable<T>(queryable, visitor);
    }
}
