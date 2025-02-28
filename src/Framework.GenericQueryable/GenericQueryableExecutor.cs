using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.GenericQueryable;

public abstract class GenericQueryableExecutor : IGenericQueryableExecutor
{
    protected abstract Type ExtensionsType { get; }

    protected virtual string GetMethodName(MethodInfo methodInfo) => methodInfo.Name.Replace("Generic", "");

    protected virtual IReadOnlyCollection<Expression> GetArguments(MethodCallExpression methodCallExpression)
    {
        return methodCallExpression.Arguments;
    }

    protected virtual MethodInfo GetTargetMethod(MethodCallExpression methodCallExpression, string targetMethodName, IReadOnlyList<Type> argTypes)
    {
        var genericTargetMethod =

            this.ExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == targetMethodName && m.GetParameters().Length == argTypes.Count);

        return genericTargetMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());
    }

    protected virtual object InvokeMethod(MethodInfo targetMethod, IReadOnlyList<object> args) =>
        targetMethod.Invoke(null, args.ToArray())!;

    public object Execute(GenericQueryableExecuteExpression genericQueryableExecuteExpression)
    {
        var methodCallExpression = (MethodCallExpression)genericQueryableExecuteExpression.CallExpression.Body;

        var argExpressions = this.GetArguments(methodCallExpression);

        var args = argExpressions.Select(arg => arg.GetMemberConstValue().GetValue()).ToArray();

        var targetMethod = this.GetTargetMethod(
            methodCallExpression,
            this.GetMethodName(methodCallExpression.Method),
            argExpressions.Select(argExpr => argExpr.Type).ToArray());

        return this.InvokeMethod(targetMethod, args);
    }
}
