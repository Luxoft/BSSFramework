using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven._Visitors;

internal class OverrideIdEqualsMethodVisitor<TIdent> : ExpressionVisitor
{
    private static readonly MethodInfo ObjectEqualsMethod = typeof(object).GetMethod(nameof(object.Equals), new[] { typeof(object) });
    private static readonly MethodInfo GenericEqualsMethod = typeof(TIdent).GetMethod(nameof(object.Equals), new[] { typeof(TIdent) });

    private static readonly OverrideIdEqualsMethodVisitor<TIdent> Instance = new OverrideIdEqualsMethodVisitor<TIdent>();

    private OverrideIdEqualsMethodVisitor()
    {
    }

    /// <summary> Returns <see cref="OverrideIdEqualsMethodVisitor{TIdent}"/> for specified <paramref name="property"/>
    /// </summary>
    /// <param name="property">Property to get ExpressionVisitor for (not used, included for compatibility)</param>
    /// <returns>Expression Visitor</returns>
    public static OverrideIdEqualsMethodVisitor<TIdent> GetOrCreate(PropertyInfo property)
    {
        // HACK gtsaplin: property included for compatibility
        return Instance;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        Expression result;
        if (node.Method == ObjectEqualsMethod || node.Method == GenericEqualsMethod)
        {
            var left = this.Visit(node.Object);
            var arg = this.Visit(node.Arguments.Single());
            var right = arg.ExtractBoxingValue();
            result = Expression.MakeBinary(ExpressionType.Equal, left, right);
        }
        else
        {
            result = base.VisitMethodCall(node);
        }

        return result;
    }
}
