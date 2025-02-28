using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.GenericQueryable.Default;

public class DefaultGenericQueryableExecutor : GenericQueryableExecutor
{
    protected override Type ExtensionsType { get; } = typeof(Queryable);

    protected override string GetMethodName(MethodInfo methodInfo) => base.GetMethodName(methodInfo).SkipLast("Async", true);

    protected override IReadOnlyCollection<Expression> GetArguments(MethodCallExpression methodCallExpression)
    {
        var baseArguments = base.GetArguments(methodCallExpression);

        if (baseArguments.Last().Type == typeof(CancellationToken))
        {
            return baseArguments.Take(baseArguments.Count - 1).ToArray();
        }
        else
        {
            return baseArguments;
        }
    }
    protected override MethodInfo GetTargetMethod(MethodCallExpression methodCallExpression, string targetMethodName, IReadOnlyList<Type> argTypes)
    {
        if (targetMethodName == "ToList")
        {
            return typeof(Enumerable).GetMethod(nameof(Enumerable.ToList))!.MakeGenericMethod(
                methodCallExpression.Method.GetGenericArguments());
        }
        else
        {
            return base.GetTargetMethod(methodCallExpression, targetMethodName, argTypes);
        }
    }
}
