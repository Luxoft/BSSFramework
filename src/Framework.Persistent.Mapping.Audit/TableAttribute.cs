namespace Framework.Persistent.Mapping;

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public string Name
    {
        get;
        set;
    }

    public string Schema
    {
        get;
        set;
    }
}
