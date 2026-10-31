namespace Framework.BLL.Domain.IdentityObject;

/// <summary>
/// Interface for UpdateDTO
/// </summary>
public interface IUpdateDTO
{
    /// <summary>
    /// DTO is empty (should be checked after compression)
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Field compression
    /// </summary>
    void Compress();
}
