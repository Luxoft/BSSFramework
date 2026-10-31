namespace Framework.Relations;

/// <summary>
/// Describes a detail in a master-detail relationship
/// </summary>
/// <typeparam name="TMaster">Master type</typeparam>
public interface IDetail<out TMaster>
        where TMaster : class
{
    /// <summary>
    /// Reference to the master
    /// </summary>
    TMaster? Master { get; }
}
