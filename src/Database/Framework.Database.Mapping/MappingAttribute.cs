namespace Framework.Database.Mapping;

[AttributeUsage(AttributeTargets.Property)]
public class MappingAttribute : Attribute
{
    private CascadeMode? actualCascadeMode;

    /// <summary>
    /// Column name in the db
    /// </summary>
    public string? ColumnName
    {
        get;
        set;
    }

    public bool IsOneToOne
    {
        get;
        set;
    }

    public bool IsUnique
    {
        get;
        set;
    }

    /// <summary>
    /// Support for cascade deletion
    /// </summary>
    public CascadeMode CascadeMode
    {
        get => this.actualCascadeMode ?? CascadeMode.Auto;
        set => this.actualCascadeMode = value;
    }

    public CascadeMode? GetActualCascadeMode() => this.actualCascadeMode;

    /// <summary>
    /// Name from a third-party table joined by id from which this column is taken
    /// </summary>
    public string? ExternalTableName { get; set; }
}
