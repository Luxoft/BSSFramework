namespace Framework.Database.Mapping;

[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute : Attribute
{
    public string Name { get; set; }
}
