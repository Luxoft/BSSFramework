using CommonFramework;

namespace Framework.Core;

/// <summary>
/// Класс, для автоматического создания Lazy-адаптера интерфейса
/// </summary>
public static class LazyInterfaceImplementHelper
{
    /// <summary>
    /// Создание фекового адаптера (без имплементации)
    /// </summary>
    /// <typeparam name="T">Тип интерфейса</typeparam>
    /// <returns></returns>
    public static T CreateNotImplemented<T>(string message = null)
    {
        return LazyInterfaceImplementHelper<T>.CreateProxy(() => { throw new NotImplementedException(message); });
    }

    /// <summary>
    /// Создание адаптера
    /// </summary>
    /// <typeparam name="T">Тип интерфейса</typeparam>
    /// <param name="factory">Фабричная функция</param>
    /// <returns></returns>
    public static T CreateProxy<T>(Func<T> factory)
    {
        var func = factory.WithCache();

        return LazyInterfaceImplementHelper<T>.CreateProxy(func);
    }

    public static T CreateCallProxy<T>(Func<T> func)
    {
        return LazyInterfaceImplementHelper<T>.CreateCallProxy(func);
    }
}

/// <summary>
/// Класс, для автоматического создания Lazy-адаптера интерфейса
/// </summary>
/// <typeparam name="T">Тип интерфейса</typeparam>
public static class LazyInterfaceImplementHelper<T>
{
    public static readonly Func<Func<T>, T> CreateProxy = LazyInterfaceImplementTypeBuilder.Default.GetCreateProxyFunc<T>();

    public static readonly Func<Func<T>, T> CreateCallProxy = CallProxyInterfaceImplementTypeBuilder.Default.GetCreateProxyFunc<T>();
}
