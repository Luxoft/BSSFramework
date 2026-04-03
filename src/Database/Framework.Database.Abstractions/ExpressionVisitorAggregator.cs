using System.Linq.Expressions;
using Framework.Core;

namespace Framework.Database;

public abstract class ExpressionVisitorAggregator : IExpressionVisitorContainer
{
    private readonly Lazy<ExpressionVisitor> lazyVisitor;

    protected ExpressionVisitorAggregator() => this.lazyVisitor = new(() => this.GetVisitors().ToComposite());

    public ExpressionVisitor Visitor => this.lazyVisitor.Value;

    protected abstract IEnumerable<ExpressionVisitor> GetVisitors();
}
