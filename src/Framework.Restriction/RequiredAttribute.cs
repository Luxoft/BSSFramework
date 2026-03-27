namespace Framework.Restriction;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute, IRestrictionAttribute
{
    public RequiredMode Mode { get; set; }
}
