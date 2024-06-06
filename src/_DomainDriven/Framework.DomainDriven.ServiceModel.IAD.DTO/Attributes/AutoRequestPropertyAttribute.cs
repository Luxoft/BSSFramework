namespace Framework.DomainDriven.ServiceModel.IAD;

[AttributeUsage(AttributeTargets.Property)]
public class AutoRequestPropertyAttribute : Attribute
{
    public int OrderIndex
    {
        get;
        set;
    }
}
