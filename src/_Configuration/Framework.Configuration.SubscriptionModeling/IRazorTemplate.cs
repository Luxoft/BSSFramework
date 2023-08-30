namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
///     Интерфейс Razor шаблона сообщения.
/// </summary>
public interface IRazorTemplate
{
    /// <summary>
    ///     Получает тему сообщения.
    /// </summary>
    /// <value>
    ///     Тема сообщения.
    /// </value>
    string Subject { get; }

    /// <summary>
    ///     Запускает экземпляр шаблона на исполнение.
    /// </summary>
    void Execute();

    /// <summary>
    ///     Устанавливает экземпляр <see cref="TextWriter" />,
    ///     который будет использован для записи генерируемого шаблоном текста.
    /// </summary>
    /// <param name="writer">Экземпляр <see cref="TextWriter" />.</param>
    void SetWriter(TextWriter writer);
}
