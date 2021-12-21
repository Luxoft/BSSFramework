namespace Framework.Configuration.Domain
{

    /// <summary>
    /// Константы, описывающие состояние регулярной задачи
    /// </summary>
    public enum RegularJobStatus
    {
        /// <summary>
        /// Инициирован
        /// </summary>
        Initial,

        /// <summary>
        /// Успешно завершен
        /// </summary>
        Successed,

        /// <summary>
        /// Не завершён c ошибкой
        /// </summary>
        Fail,
    }
}



