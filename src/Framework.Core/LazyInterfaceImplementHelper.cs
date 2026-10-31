using Anch.Core;

namespace Framework.Core;

/// <summary>
/// Class for automatic creation of a Lazy adapter of an interface
/// </summary>
public static class LazyInterfaceImplementHelper
{
    /// <summary>
    /// Creating a fake adapter (without implementation)
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <returns></returns>
    public static T CreateNotImplemented<T>(string? message = null) => LazyInterfaceImplementHelper<T>.CreateProxy(() => throw new NotImplementedException(message));

    /// <summary>
    /// Adapter creation
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="factory">Factory function</param>
    /// <returns></returns>
    public static T CreateProxy<T>(Func<T> factory)
    {
        var func = factory.WithCache();

        return LazyInterfaceImplementHelper<T>.CreateProxy(func);
    }

    public static T CreateCallProxy<T>(Func<T> func) => LazyInterfaceImplementHelper<T>.CreateCallProxy(func);
}

/// <summary>
/// Class for automatic creation of a Lazy adapter of an interface
/// </summary>
/// <typeparam name="T">Interface type</typeparam>
public static class LazyInterfaceImplementHelper<T>
{
    public static readonly Func<Func<T>, T> CreateProxy = LazyInterfaceImplementTypeBuilder.Default.GetCreateProxyFunc<T>();

    public static readonly Func<Func<T>, T> CreateCallProxy = CallProxyInterfaceImplementTypeBuilder.Default.GetCreateProxyFunc<T>();
}
