namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Attribute allowing object creation
/// </summary>
public interface IAllowCreateAttribute
{
    /// <summary>
    /// Permission to create an object
    /// </summary>
    bool AllowCreate { get; }
}
