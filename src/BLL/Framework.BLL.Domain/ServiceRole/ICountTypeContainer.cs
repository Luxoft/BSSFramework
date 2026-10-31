namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Container for marking quantitative processing
/// </summary>
public interface ICountTypeContainer
{
    /// <summary>
    /// Processing of a single object and/or a collection of objects
    /// </summary>
    CountType CountType { get; }
}
