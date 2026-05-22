using System.Collections.Concurrent;
using System.Reflection;

namespace Anch.Testing.Xunit.Engine;

public static class MethodInfoExtensions
{
    private static readonly ConcurrentDictionary<MethodInfo, bool> IsCtCache = [];

    public static bool LastParameterIsCt(this MethodInfo methodInfo)
    {
        return IsCtCache.GetOrAdd(methodInfo, m => m.GetParameters().LastOrDefault()?.ParameterType == typeof(CancellationToken));
    }
}