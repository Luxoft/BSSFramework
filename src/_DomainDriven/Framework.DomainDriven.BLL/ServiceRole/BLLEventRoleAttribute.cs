using System;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Маркер объектов отправляющих евенты в интеграцию
    /// </summary>
    public class BLLEventRoleAttribute : Attribute
    {
        /// <summary>
        /// Пользовательский enum-тип с перечнем евентов
        /// </summary>
        public Type EventOperationType { get; set; } = typeof(BLLBaseOperation);

        /// <summary>
        /// Фильтрация оправляемых евентов
        /// </summary>
        public EventRoleMode Mode { get; set; } = EventRoleMode.ALL;
    }
}
