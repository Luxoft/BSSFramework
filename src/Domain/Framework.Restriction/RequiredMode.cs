namespace Framework.Restriction;

/// <summary>
/// Additional validation modes
/// </summary>
public enum RequiredMode
{
    Default,

    /// <summary>
    /// Allows an empty string, only the null value is checked
    /// </summary>
    AllowEmptyString,

    /// <summary>
    /// Requires the period to be strictly closed, i.e. Period.EndDate != null
    /// </summary>
    ClosedPeriodEndDate
}
