namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Object save type
/// </summary>
[Flags]
public enum BLLSaveType
{
    /// <summary>
    /// Regular saving through the full Strict model
    /// </summary>
    Save = 1,

    /// <summary>
    /// Update via an Update model
    /// </summary>
    Update = 2,

    /// <summary>
    /// Both save types
    /// </summary>
    Both = Save + Update
}
