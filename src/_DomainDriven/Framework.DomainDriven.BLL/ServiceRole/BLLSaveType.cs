using System;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Тип сохранения объекта
    /// </summary>
    [Flags]
    public enum BLLSaveType
    {
        /// <summary>
        /// Обычное сохранение через полную Strict-модель
        /// </summary>
        Save = 1,

        /// <summary>
        /// Обновление через Update-модель
        /// </summary>
        Update = 2,

        /// <summary>
        /// Оба типа сохранения
        /// </summary>
        Both = Save + Update
    }
}
