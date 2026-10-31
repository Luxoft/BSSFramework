using Framework.BLL.Domain.Persistent.Attributes;

namespace Framework.BLL.Domain.Fetching;

/// <summary>
/// Path attribute for generating Fetches
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class FetchPathAttribute(string path) : PathAttribute(path);
