namespace Framework.BLL.Domain.DirectMode;

/// <summary>
/// Model-based generation control parameters
/// </summary>
[Flags]
public enum DirectMode
{
    /// <summary>
    /// The model is returned as the result of the method
    /// </summary>
    Out = 1,

    /// <summary>
    /// The model is accepted as a method parameter
    /// </summary>
    In = 2,
}
