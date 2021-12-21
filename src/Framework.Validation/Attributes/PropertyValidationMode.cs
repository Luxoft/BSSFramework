namespace Framework.Validation
{
    /// <summary>
    /// Режим валидации свойства
    /// </summary>
    public enum PropertyValidationMode
    {
        /// <summary>
        /// Валидация принудительно выключена
        /// </summary>
        Disabled,

        /// <summary>
        /// Режим валидации выбирается автоматически на основе внутренних политик
        /// </summary>
        Auto,

        /// <summary>
        /// Валидация принудительно включена
        /// </summary>
        Enabled
    }
}