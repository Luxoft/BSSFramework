using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core;

public class OverrideStringEqualityExpressionVisitor : ExpressionVisitor
{
    private static readonly MethodInfo StringEqualityMethod = typeof(string).GetEqualityMethod();

    private static readonly MethodInfo StringEqualsMethod = new Func<string, string, StringComparison, bool>(string.Equals).Method;


    private static readonly Dictionary<MethodInfo, MethodInfo> CallMap = new Dictionary<MethodInfo, MethodInfo>
                                                                         {
                                                                                 { new Func<string, bool>("".Contains).Method, new Func<string, StringComparison, bool>("".Contains).Method },
                                                                                 { new Func<string, bool>("".StartsWith).Method, new Func<string, StringComparison, bool>("".StartsWith).Method },
                                                                                 { new Func<string, bool>("".EndsWith).Method, new Func<string, StringComparison, bool>("".EndsWith).Method }
                                                                         };


    private readonly Expression _stringComparisonExpr;


    public OverrideStringEqualityExpressionVisitor(StringComparison stringComparison)
    {
        this._stringComparisonExpr = Expression.Constant(stringComparison);
    }


    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var request = from targetMethod in CallMap.GetMaybeValue(node.Method)

                      let input = node.GetChildren().Select(this.Visit).ToArray()

                      let newObj = targetMethod.IsStatic ? null : input.First()

                      let newArgs = targetMethod.IsStatic ? input : input.Skip(1)

                      select Expression.Call(newObj, targetMethod, newArgs.Concat(new[] { this._stringComparisonExpr }));


        return request.GetValueOrDefault(() => base.VisitMethodCall(node));
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.Method == StringEqualityMethod)
        {
            return Expression.Call(StringEqualsMethod, node.Left, node.Right, this._stringComparisonExpr);
        }

        return base.VisitBinary(node);
    }
}
