namespace Framework.DomainDriven.Serialization;

[AttributeUsage(AttributeTargets.Property)]
public class AutoMappingAttribute(bool enabled) : Attribute
{
    public bool Enabled { get; } = enabled;
}
