namespace Framework.Database.Domain;

[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute :Attribute
{
    public string Name
    {
        get;
        set;
    }
}
