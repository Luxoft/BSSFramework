namespace Framework.Restriction;

/// <summary>
///     Uniqueness check by a set of fields. Generates a constraint on the table. Applied both to the object itself and to
///     collections in it.
/// </summary>
/// <seealso cref="System.Attribute" />
/// <seealso cref="Framework.Restriction.IUniqueAttribute" />
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
public class UniqueGroupAttribute(string? key = null) : Attribute, IUniqueAttribute
{
    /// <summary>
    ///     Returns the string key of the group.
    /// </summary>
    /// <value>
    ///     String key of the group.
    /// </value>
    public string? Key { get; } = key;

    /// <summary>
    ///     Gets or sets a flag indicating that uniqueness must be checked by querying the database.
    /// </summary>
    /// <value>
    ///     <c>true</c> if uniqueness must be checked by querying the database; otherwise, <c>false</c>.
    /// </value>
    public bool UseDbEvaluation { get; set; }
}
