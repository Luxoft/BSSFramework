using System;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Интерфейс доменного типа авторизации для типизированного контекста.
    /// </summary>
    public interface ISecurityContext
    {
        /// <summary>
        /// Возвращает тип доменного типа.
        /// </summary>
        /// <returns>Тип доменного типа.</returns>
        Type GetType();
    }
}
