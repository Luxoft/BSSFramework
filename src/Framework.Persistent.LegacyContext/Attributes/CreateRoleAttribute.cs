namespace Framework.Persistent;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class CreateRoleAttribute : Attribute
{
    public CreateRoleAttribute(bool value)
    {
        this.Value = value;
    }


    public bool Value { get; private set; }
}
