namespace Framework.Restriction;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class UniqueElementAttribute : Attribute, IUniqueAttribute
{
    public UniqueElementAttribute()
            : this(null)
    {

    }

    public UniqueElementAttribute(string key)
    {
        this.Key = key;
    }


    public string Key { get; private set; }
}
