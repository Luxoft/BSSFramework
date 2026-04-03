namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class CustomNameAttribute(string name) : Attribute
{
    public string Name { get; private set; } = name;
}
