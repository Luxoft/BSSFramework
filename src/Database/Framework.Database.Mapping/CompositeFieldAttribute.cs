namespace Framework.Database.Mapping;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CompositeFieldAttribute : Attribute
{
    public string ClassFieldName = null!;

    public string? ColumnName;
}
