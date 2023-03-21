using System;
using System.Reflection;

namespace Framework.Core;

public static class MethodInfoExtensions
{
    public static bool IsGenericMethodImplementation(this MethodInfo methodInfo, MethodInfo genericMethod)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
        if (genericMethod == null) throw new ArgumentNullException(nameof(genericMethod));

        if (!genericMethod.IsGenericMethodDefinition)
        {
            throw new ArgumentOutOfRangeException(nameof(genericMethod));
        }

        return methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == genericMethod;
    }
}
