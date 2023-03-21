using System;

namespace Framework.Core;

public static class LambdaCompileCacheContainerExtensions
{
    public static ILambdaCompileCache Get<TRoute1>(this ILambdaCompileCacheContainer container)
    {
        if (container == null) throw new ArgumentNullException(nameof(container));

        return container[typeof(TRoute1)];
    }

    public static ILambdaCompileCache Get<TRoute1, TRoute2>(this ILambdaCompileCacheContainer container)
    {
        if (container == null) throw new ArgumentNullException(nameof(container));

        return container[typeof(TRoute1), typeof(TRoute2)];
    }
}
