namespace Framework.Persistent.Mapping;

[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute :Attribute
{
    public string Name
    {
        get;
        set;
    }
}
