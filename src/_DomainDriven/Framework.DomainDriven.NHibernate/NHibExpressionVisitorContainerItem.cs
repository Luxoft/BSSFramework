using System.Linq.Expressions;

using Framework.DomainDriven._Visitors;

namespace Framework.DomainDriven.NHibernate;

public class NHibExpressionVisitorContainerItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return FixNHibArrayContainsVisitor.Value;
    }
}
