namespace Framework.Relations;

/// <summary>
/// Describes a master object in a master-detail relationship
/// </summary>
/// <typeparam name="TDetail">Detail type</typeparam>
public interface IMaster<TDetail>
{
    /// <summary>
    /// Reference to the details collection
    /// </summary>
    ICollection<TDetail> Details { get; }
}
