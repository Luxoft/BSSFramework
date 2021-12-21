namespace Framework.Configuration.Domain
{

    /// <summary>
    /// Константы, описывающие состояние регулярной задачи
    /// </summary>
    public enum RegularJobState
    {
        /// <summary>
        /// Ожидает оповещение о смене времени
        /// </summary>
        /// <remarks>Оповещение идет через biztalk</remarks>
        WaitPulse,

        /// <summary>
        /// Выполняется
        /// </summary>
        Running,

        /// <summary>
        /// Отправлено на выполнение
        /// </summary>
        SendToProcessing
    }
}
