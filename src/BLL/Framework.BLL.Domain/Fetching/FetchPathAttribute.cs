using Framework.BLL.Domain.Persistent.Attributes;

namespace Framework.BLL.Domain.Fetching;

/// <summary>
/// Атрибут пути, для генерации Fetch-ей
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class FetchPathAttribute(string path) : PathAttribute(path);
