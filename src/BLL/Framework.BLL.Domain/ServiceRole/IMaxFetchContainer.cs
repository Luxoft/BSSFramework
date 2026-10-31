using Framework.BLL.Domain.DTO;

namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Container for marking the size of data fetched from the database
/// </summary>
public interface IMaxFetchContainer
{
    /// <summary>
    /// Maximum database fetch level
    /// </summary>
    ViewDTOType MaxFetch { get; }
}
