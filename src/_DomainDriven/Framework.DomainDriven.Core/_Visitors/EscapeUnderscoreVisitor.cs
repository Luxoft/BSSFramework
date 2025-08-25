using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;
using CommonFramework.Maybe;

namespace Framework.DomainDriven._Visitors;

public class EscapeUnderscoreVisitor : ExpressionVisitor
{
    private static readonly MethodInfo StringContainsMethod = new Func<string, bool>("".Contains).Method;


    private EscapeUnderscoreVisitor()
    {

    }


    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var visitRequest = from _ in Maybe.Return()

                           where node.Method == StringContainsMethod

                           from value in node.Arguments.Single().GetDeepMemberConstValue<string>()

                           where !value.Contains("[_]") && value.Contains("_")

                           let nextValue = value.Replace("_", "[_]")

                           select Expression.Call(node.Object, node.Method, Expression.Constant(nextValue));


        return base.VisitMethodCall(visitRequest.GetValueOrDefault(node));
    }


    public static readonly EscapeUnderscoreVisitor Value = new EscapeUnderscoreVisitor();
}
