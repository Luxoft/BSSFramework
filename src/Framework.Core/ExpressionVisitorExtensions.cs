using System.Linq.Expressions;

using CommonFramework;

namespace Framework.Core;

public static class ExpressionVisitorExtensions
{
    public static ExpressionVisitor ToComposite(this ExpressionVisitor visitor, params ExpressionVisitor[] otherVisitors)
    {
        if (visitor == null) throw new ArgumentNullException(nameof(visitor));
        if (otherVisitors == null) throw new ArgumentNullException(nameof(otherVisitors));

        return new[] { visitor }.Concat(otherVisitors).ToComposite();
    }

    public static ExpressionVisitor ToComposite(this IEnumerable<ExpressionVisitor> visitors)
    {
        if (visitors == null) throw new ArgumentNullException(nameof(visitors));

        return visitors.Match(
                              () => EmptyExpressionVisitor.Value,
                              visitor => visitor,
                              visitorsArr => new CompositeExpressionVisitor(visitorsArr));
    }

    public static ExpressionVisitor ToCyclic(this ExpressionVisitor visitor, params ExpressionVisitor[] otherVisitors)
    {
        if (visitor == null) throw new ArgumentNullException(nameof(visitor));
        if (otherVisitors == null) throw new ArgumentNullException(nameof(otherVisitors));

        return new[] { visitor }.Concat(otherVisitors).ToCyclic();
    }

    public static ExpressionVisitor ToCyclic(this IEnumerable<ExpressionVisitor> visitors)
    {
        if (visitors == null) throw new ArgumentNullException(nameof(visitors));

        return new CyclicExpressionVisitor(visitors);
    }

    private class CompositeExpressionVisitor : ExpressionVisitor
    {
        private readonly IReadOnlyCollection<ExpressionVisitor> visitors;

        public CompositeExpressionVisitor(ExpressionVisitor[] visitors)
        {
            this.visitors = visitors ?? throw new ArgumentNullException(nameof(visitors));
        }

        public override Expression Visit(Expression node)
        {
            return node.UpdateBase(this.visitors);
        }
    }

    private class CyclicExpressionVisitor : ExpressionVisitor
    {
        private readonly IReadOnlyCollection<ExpressionVisitor> visitors;

        public CyclicExpressionVisitor(IEnumerable<ExpressionVisitor> visitors)
        {
            if (visitors == null) throw new ArgumentNullException(nameof(visitors));

            this.visitors = visitors.ToArray();
        }

        public override Expression Visit(Expression node)
        {
            var res = node.UpdateBase(this.visitors);

            return node == res ? res : this.Visit(res);
        }
    }
}
