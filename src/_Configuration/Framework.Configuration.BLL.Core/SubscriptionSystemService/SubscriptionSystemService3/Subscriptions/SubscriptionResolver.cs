using System;
using System.Collections.Generic;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
/// Базовый класс, определяющий контракт типа, предназначенного для поиска подписок.
/// </summary>
public abstract class SubscriptionResolver
{
    /// <summary>Выполняет поиск всех подписок, привязанных к конкретному типу доменного объекта.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Список подписок для конкретного типа доменного объекта.</returns>
    public abstract IEnumerable<Subscription> Resolve<T>(DomainObjectVersions<T> versions)
            where T : class;

    /// <summary>Выполняет поиск подписки для переданного кода подписки.</summary>
    /// <typeparam name="T">Тип доменного объекта</typeparam>
    /// <param name="subscriptionCode">Код подписки.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Подписку, найденную по переданному коду подписки.</returns>
    public abstract Subscription Resolve<T>(string subscriptionCode, DomainObjectVersions<T> versions)
            where T : class;

    /// <summary>
    /// Определяет, что для заданного типа существуют активные подписки.
    /// </summary>
    /// <param name="domainObjectType">Тип для которого будет производиться поиск активных подписок.</param>
    /// <returns>
    ///   <c>true</c> если для переданного типа найдены активные подписки; в противном случае <c>false</c>.
    /// </returns>
    public abstract bool IsActiveSubscriptionForTypeExists(Type domainObjectType);
}
