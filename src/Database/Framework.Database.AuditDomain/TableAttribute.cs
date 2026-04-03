namespace Framework.Persistent.Mapping;

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public required string Name
    {
        get;
        init;
    }

    public string? Schema
    {
        get;
        init;
    }
}
