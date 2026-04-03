namespace Framework.BLL.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class CreateRoleAttribute(bool value) : Attribute
{
    public bool Value { get; private set; } = value;
}
