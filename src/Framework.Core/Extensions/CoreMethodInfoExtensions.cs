using System.Reflection;

namespace Framework.Core;

public static class CoreMethodInfoExtensions
{
    public static TDelegate ToDelegate<TDelegate>(this MethodInfo methodInfo)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

        return (TDelegate)(object)methodInfo.ToDelegate(typeof(TDelegate));
    }

    public static Delegate ToDelegate(this MethodInfo methodInfo, Type delegateType)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

        return Delegate.CreateDelegate(delegateType, methodInfo);
    }

    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source)
    {
        return methodInfo.Invoke<TResult>(source, new object[0]);
    }

    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source, IEnumerable<object> args)
    {
        return (TResult)methodInfo.Invoke(source, args.ToArray());
    }

    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source, object arg1, params object[] args)
    {
        return methodInfo.Invoke<TResult>(source, new[] { arg1 }.Concat(args));
    }

    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source, object arg1, object arg2, params object[] args)
    {
        return methodInfo.Invoke<TResult>(source, new[] { arg1, arg2 }.Concat(args));
    }

    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source, object arg1, object arg2, object arg3, params object[] args)
    {
        return methodInfo.Invoke<TResult>(source, new[] { arg1, arg2, arg3 }.Concat(args));
    }
}
