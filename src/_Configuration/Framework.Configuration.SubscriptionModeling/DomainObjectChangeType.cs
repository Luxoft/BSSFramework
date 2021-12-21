using System;

namespace Framework.Configuration.SubscriptionModeling
{
    /// <summary>
    /// Описывает тип изменения доменного объекта для которого должна срабатывать подписка.
    /// </summary>
    [Flags]
    public enum DomainObjectChangeType
    {
        /// <summary>
        /// Любое изменение.
        /// </summary>
        Any = 1,

        /// <summary>
        /// Создание доменного объекта.
        /// </summary>
        Create = 2,

        /// <summary>
        /// Удаление доменного объекта.
        /// </summary>
        Delete = 4,

        /// <summary>
        /// Изменение доменного объекта.
        /// </summary>
        Update = 8,

        /// <summary>
        /// Создание или изменение доменного объекта.
        /// </summary>
        CreateOrUpdate = Create | Update,

        /// <summary>
        /// Изменение или удаление доменного объекта.
        /// </summary>
        UpdateOrDelete = Update | Delete
    }
}