namespace Framework.BLL.DTOMapping.Domain;

[AttributeUsage(AttributeTargets.Property)]
public class AutoRequestPropertyAttribute : Attribute
{
    public int OrderIndex
    {
        get;
        set;
    }
}
