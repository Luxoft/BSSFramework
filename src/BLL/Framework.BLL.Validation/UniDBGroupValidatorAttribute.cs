using Framework.Validation.Attributes;
using Framework.Validation.Validators;

namespace Framework.BLL.Validation;

/// <summary>
/// Attribute for uniqueness check by querying the database.
/// </summary>
/// <seealso cref="ClassValidatorAttribute" />
[AttributeUsage(AttributeTargets.Class)]
public class UniDBGroupValidatorAttribute : ClassValidatorAttribute
{
    /// <summary>
    ///     Returns the string key of the group.
    /// </summary>
    /// <value>
    ///     String key of the group.
    /// </value>
    public string? GroupKey { get; init; }

    /// <summary>
    ///     Gets or sets a flag indicating that uniqueness must be checked by querying the database.
    /// </summary>
    /// <value>
    ///     <c>true</c> if uniqueness must be checked by querying the database; otherwise, <c>false</c>.
    /// </value>
    public bool UseDbEvaluation { get; set; }

    /// <inheritdoc />
    public override IClassValidator CreateValidator() => new UniqueGroupDatabaseValidator(this.GroupKey);
}
