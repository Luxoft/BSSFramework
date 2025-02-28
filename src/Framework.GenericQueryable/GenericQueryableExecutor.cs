using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.GenericQueryable;

public abstract class GenericQueryableExecutor : IGenericQueryableExecutor
{
    private readonly IDictionaryCache<MethodInfo, MethodInfo> mappingMethodCache;

    protected GenericQueryableExecutor() =>
        this.mappingMethodCache = new DictionaryCache<MethodInfo, MethodInfo>(this.GetTargetMethod).WithLock();

    protected abstract Type ExtensionsType { get; }

    protected virtual string GetTargetMethodName(MethodInfo baseMethod) => baseMethod.Name.Skip("Generic", true);

    protected virtual int GetParameterCount(MethodInfo baseMethod)
    {
        return baseMethod.GetParameters().Length;
    }

    protected virtual MethodInfo GetTargetMethod(MethodInfo baseMethod)
    {
        var targetMethodName = this.GetTargetMethodName(baseMethod);

        var genericArgs = baseMethod.GetGenericArguments();

        var parameterTypes = baseMethod.GetParameters().Take(this.GetParameterCount(baseMethod)).Select(p => p.ParameterType).ToArray();

        var request = from method in this.ExtensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)

                      where method.Name == targetMethodName && method.GetGenericArguments().Length == genericArgs.Length

                      let genMethod = method.MakeGenericMethod(genericArgs)

                      where genMethod.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes)

                      select genMethod;

        return request.Single();
    }

    public virtual object Execute(GenericQueryableExecuteExpression genericQueryableExecuteExpression)
    {
        var methodCallExpression = (MethodCallExpression)genericQueryableExecuteExpression.CallExpression.Body;

        var args = methodCallExpression
                   .Arguments
                   .Take(this.GetParameterCount(methodCallExpression.Method))
                   .Select(arg => arg.GetMemberConstValue().GetValue()).ToArray();

        return this.mappingMethodCache[methodCallExpression.Method].Invoke(null, args)!;
    }
}
