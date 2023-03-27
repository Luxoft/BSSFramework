namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class CustomNameAttribute : Attribute
{
    public CustomNameAttribute(string name)
    {
        this.Name = name;
    }


    public string Name { get; private set; }
}
