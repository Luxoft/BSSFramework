using System.Linq.Expressions;

namespace Framework.Database.NHibernate;

public class FixNHibArrayContainsVisitorContainer : IExpressionVisitorContainer
{
    public ExpressionVisitor Visitor { get; } = new FixNHibArrayContainsVisitor();
}
