using System.Reflection;

namespace Framework.Core;

public static class MethodInfoExtensions
{
    public static Delegate ToDelegate(this MethodInfo methodInfo, Type delegateType)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

        return Delegate.CreateDelegate(delegateType, methodInfo);
    }
}
