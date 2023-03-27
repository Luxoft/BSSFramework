namespace Framework.Persistent.Mapping;

/// <summary>
/// Update, Insert (optional - defaults to true): specifies that the mapped columns should be included in SQL UPDATE and/or INSERT statements. Setting both to false allows a pure "derived" property whose value is initialized from some other property that maps to the same column(s) or by a trigger or other application.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MappingPropertyAttribute : Attribute
{
    public MappingPropertyAttribute()
    {
    }

    public MappingPropertyAttribute(bool canInsert, bool canUpdate)
    {
        this.CanUpdate = canUpdate;
        this.CanInsert = canInsert;
    }

    /// <summary>
    /// Specifies that the mapped columns should be included in SQL INSERT statements
    /// </summary>
    public bool CanInsert { get; set; }

    /// <summary>
    /// Specifies that the mapped columns should be included in SQL UPDATE statements
    /// </summary>
    public bool CanUpdate { get; set; }
}
