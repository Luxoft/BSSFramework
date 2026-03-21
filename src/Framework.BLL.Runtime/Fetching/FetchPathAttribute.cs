using Framework.BLL.Domain.Persistent.Attributes;

namespace Framework.BLL.Fetching;

/// <summary>
/// Атрибут пути, для генерации Fetch-ей
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class FetchPathAttribute : PathAttribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Путь</param>
    public FetchPathAttribute(string path)
            : base(path)
    {
    }
}
