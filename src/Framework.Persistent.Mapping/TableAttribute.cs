namespace Framework.Persistent.Mapping;

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : NamedAttribute
{
    public string Schema
    {
        get;
        set;
    }
}
