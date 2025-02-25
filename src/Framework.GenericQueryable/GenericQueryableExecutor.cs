using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.GenericQueryable;

public abstract class GenericQueryableExecutor : IGenericQueryableExecutor
{
    protected abstract Type ExtensionsType { get; }

    protected virtual string GetMethodName(MethodInfo methodInfo) => methodInfo.Name.Replace("Generic", "");

    public object Execute(GenericQueryableMethodExpression genericQueryableMethodExpression)
    {
        var methodCallExpression = (MethodCallExpression)genericQueryableMethodExpression.CallExpression.Body;

        var targetMethodName = this.GetMethodName(methodCallExpression.Method);

        var genericTargetMethod =

            this.ExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == targetMethodName && m.GetParameters().Length == methodCallExpression.Arguments.Count);

        var targetMethod = genericTargetMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

        var args = methodCallExpression.Arguments.Select(arg => arg.GetMemberConstValue().GetValue()).ToArray();

        return targetMethod.Invoke(null, args)!;
    }
}
