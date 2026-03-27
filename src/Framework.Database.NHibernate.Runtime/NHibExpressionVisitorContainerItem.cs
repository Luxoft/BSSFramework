using System.Linq.Expressions;

using Framework.Application._Visitors.ExpressionVisitorContainer;

namespace Framework.Database.NHibernate;

public class NHibExpressionVisitorContainerItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return FixNHibArrayContainsVisitor.Value;
    }
}
