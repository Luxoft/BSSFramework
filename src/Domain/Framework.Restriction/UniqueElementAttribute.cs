namespace Framework.Restriction;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class UniqueElementAttribute(string? key = null) : Attribute, IUniqueAttribute
{
    public string? Key => key;
}
