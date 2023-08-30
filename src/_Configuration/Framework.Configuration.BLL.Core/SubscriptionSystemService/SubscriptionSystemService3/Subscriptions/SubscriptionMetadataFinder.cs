using System.Reflection;
using Framework.Configuration.SubscriptionModeling;
using Framework.Core;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <inheritdoc />
public class SubscriptionMetadataFinder : ISubscriptionMetadataFinder
{
    protected static KeyValuePair<object, Type> ToArgPair<T>(T obj)
    {
        return ((object)obj).ToKeyValuePair(typeof(T));
    }

    /// <inheritdoc />
    public virtual IEnumerable<ISubscriptionMetadata> Find()
    {
        return this.FindByArgs();
    }

    /// <summary>
    ///     Возвращает массив экземпляров <see cref="Assembly" />, в котором будет произведён поиск типов,
    ///     описывающих конфигурации подписок.
    ///     Значение этого свойства используется процедурой автоматической регистрации конфигураци подписок.
    /// </summary>
    /// <returns>Массив экземпляров <see cref="Assembly" />.</returns>
    protected virtual Assembly[] GetSubscriptionMetadataAssemblies()
    {
        return new Assembly[0];
    }

    /// <summary>Возвращает список имён типов, которые необходимо исключить при регистрации метаданных Code first подписок.</summary>
    /// <returns>Список имён исключаемых типов.</returns>
    protected virtual IEnumerable<string> GetSubscriptionsMetadataExclusions()
    {
        yield break;
    }

    /// <summary>
    /// Производит поиск Code first моделей подписок, где есть 1 аргумент у конструктора
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerable<ISubscriptionMetadata> Find<T1>(T1 arg1)
    {
        return this.FindByArgs(ToArgPair(arg1));
    }

    /// <summary>
    /// Производит поиск Code first моделей подписок, где есть 2 аргумента у конструктора
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerable<ISubscriptionMetadata> Find<T1, T2>(T1 arg1, T2 arg2)
    {
        return this.FindByArgs(ToArgPair(arg1), ToArgPair(arg2));
    }

    /// <summary>
    /// Производит поиск Code first моделей подписок, где есть 3 аргумента у конструктора
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerable<ISubscriptionMetadata> Find<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
    {
        return this.FindByArgs(ToArgPair(arg1), ToArgPair(arg2), ToArgPair(arg3));
    }

    /// <summary>
    /// Производит поиск Code first моделей подписок по аргументам конструктора
    /// </summary>
    /// <param name="ctorArgs">Аргументы конструктора</param>
    /// <returns></returns>
    protected IEnumerable<ISubscriptionMetadata> FindByArgs(params KeyValuePair<object, Type>[] ctorArgs)
    {
        if (ctorArgs == null) throw new ArgumentNullException(nameof(ctorArgs));

        var ignoredTypeNames = this.GetSubscriptionsMetadataExclusions().ToHashSet();

        return from assembly in this.GetSubscriptionMetadataAssemblies()

               from type in assembly.GetTypes()

               where !type.IsAbstract && typeof(ISubscriptionMetadata).IsAssignableFrom(type) && !ignoredTypeNames.Contains(type.FullName)

               let ctor = type.GetConstructor(ctorArgs.ToArray(v => v.Value))

               where ctor != null

               select (ISubscriptionMetadata)ctor.Invoke(ctorArgs.ToArray(v => v.Key));
    }
}
