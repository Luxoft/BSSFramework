using System;

namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Перечисление способов делегирования
    /// </summary>
    [Flags]
    public enum DelegatePermissionRoleLevel
    {
        /// <summary>
        /// Можно делегировать только дочерние роли
        /// </summary>
#pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
        Child,
#pragma warning restore S2346 // Flags enumerations zero-value members should be named "None"

        /// <summary>
        /// Можно делегировать дочерние роли и саму бизнес-роль
        /// </summary>
        All
    }
}
