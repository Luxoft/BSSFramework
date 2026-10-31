namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Attribute for saving an object through the client facade layer (also additionally controls CreateModelType/ChangeModelType/ExtendedModelType models)
/// </summary>
public class BLLSaveRoleAttribute : BLLServiceRoleAttribute, IAllowCreateAttribute, ICountTypeContainer
{
    /// <inheritdoc />
    public bool AllowCreate { get; set; } = true;

    /// <summary>
    /// Object saving method
    /// </summary>
    public BLLSaveType SaveType { get; set; } = BLLSaveType.Save;

    /// <summary>
    /// Saving a single object and/or a collection of objects
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
