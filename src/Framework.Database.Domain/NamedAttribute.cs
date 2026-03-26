namespace Framework.Database.Domain;

public class NamedAttribute : Attribute
{
    public string Name
    {
        get;
        set;
    }
}
