namespace Framework.Configuration.Core
{
    /// <summary>
    /// Константы, определяющие тип отчета
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// Отчеты, конструируемые во время работы приложения конечным пользователем.Основаны на домейной модели
        /// </summary>
        Persistent = 0,

        /// <summary>
        /// Отчеты, формируемые кодом
        /// </summary>
        Custom = 1
    }
}