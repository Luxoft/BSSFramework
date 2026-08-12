using System.Reflection;

namespace Framework.Core;

public static class CoreMethodInfoExtensions
{
    public static Delegate ToDelegate(this MethodInfo methodInfo, Type delegateType) => Delegate.CreateDelegate(delegateType, methodInfo);
}
