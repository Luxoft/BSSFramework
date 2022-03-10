namespace Framework.Attachments.Domain
{
    /// <summary>
    /// Аттачмент
    /// </summary>
    /// <remarks>
    /// Аттачмент прикреплен к доменному объекту и описывает файл, который был добавлен механизмом сохранения файлов
    /// </remarks>
    public interface ITemplateContainer
    {
        string Name { get; }
    }
}
