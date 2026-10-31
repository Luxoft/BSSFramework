namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
///  Attribute for deleting an object through the client facade layer
/// </summary>
public class BLLRemoveRoleAttribute : BLLServiceRoleAttribute, ICountTypeContainer
{
    /// <summary>
    /// Deleting a single object and/or a collection of objects
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
