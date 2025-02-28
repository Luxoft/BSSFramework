using System.Reflection;

using Framework.Core;

namespace Framework.GenericQueryable.Default;

public class DefaultGenericQueryableExecutor : GenericQueryableExecutor
{
    protected override Type ExtensionsType { get; } = typeof(Queryable);

    protected override string GetTargetMethodName(MethodInfo baseMethod) => base.GetTargetMethodName(baseMethod).SkipLast("Async", true);

    protected override int GetParameterCount(MethodInfo baseMethod)
    {
        if (baseMethod.GetParameters().Last().ParameterType != typeof(CancellationToken))
        {
            throw new InvalidOperationException(
                $"The last parameter of the method '{baseMethod.Name}' must be of type {nameof(CancellationToken)}.");
        }
        else
        {
            return baseMethod.GetParameters().Length - 1;
        }
    }

    protected override MethodInfo GetTargetMethod(MethodInfo baseMethod)
    {
        if (baseMethod.Name == nameof(GenericQueryableExtensions.GenericToListAsync))
        {
            return typeof(Enumerable).GetMethod(nameof(Enumerable.ToList))!.MakeGenericMethod(
                baseMethod.GetGenericArguments());
        }
        else
        {
            return base.GetTargetMethod(baseMethod);
        }
    }
}
