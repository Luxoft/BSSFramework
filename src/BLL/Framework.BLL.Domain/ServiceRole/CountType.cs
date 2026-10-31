namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Flag of the "1 or more" dimensions
/// </summary>
[Flags]
public enum CountType
{
    /// <summary>
    /// One
    /// </summary>
    Single = 1,

    /// <summary>
    /// Collection
    /// </summary>
    Many = 2,

    /// <summary>
    /// All together
    /// </summary>
    Both = Single + Many
}
