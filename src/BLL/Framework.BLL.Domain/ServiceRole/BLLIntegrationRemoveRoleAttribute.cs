namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Attribute for deleting an object through integration
/// </summary>
public class BLLIntegrationRemoveRoleAttribute : BLLServiceRoleAttribute, ICountTypeContainer
{
    /// <summary>
    /// Deleting a single object and/or a collection of objects
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
