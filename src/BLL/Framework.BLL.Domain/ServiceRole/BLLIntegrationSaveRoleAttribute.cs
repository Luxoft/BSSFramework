namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Attribute for accepting object changes through integration (also additionally controls IntegrationSaveModelType models)
/// </summary>
public class BLLIntegrationSaveRoleAttribute : BLLServiceRoleAttribute, IAllowCreateAttribute, ICountTypeContainer
{
    /// <inheritdoc />
    public bool AllowCreate { get; set; } = true;

    /// <summary>
    /// Saving a single object and/or a collection of objects
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
