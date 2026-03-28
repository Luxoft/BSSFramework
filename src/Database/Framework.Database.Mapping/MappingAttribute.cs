namespace Framework.Database.Mapping;

[AttributeUsage(AttributeTargets.Property)]
public class MappingAttribute : Attribute
{
    private CascadeMode? actualCascadeMode;

    /// <summary>
    /// Имя колонки в бд
    /// </summary>
    public string ColumnName
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
    /// Поддержка каскадности удаления
    /// </summary>
    public CascadeMode CascadeMode
    {
        get { return this.actualCascadeMode ?? CascadeMode.Auto; }
        set { this.actualCascadeMode = value; }
    }

    public CascadeMode? GetActualCascadeMode()
    {
        return this.actualCascadeMode;
    }

    /// <summary>
    /// Имя из сторонней таблицы которая join-тся по id и из которой берётся данная колонка
    /// </summary>
    public string? ExternalTableName { get; set; }
}
