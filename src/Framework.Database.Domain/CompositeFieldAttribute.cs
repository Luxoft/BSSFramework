namespace Framework.Database.Domain;

[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
public class CompositeFieldAttribute : Attribute
{
    public string ClassFieldName;

    public string ColumnName;
}
