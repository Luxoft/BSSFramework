namespace Framework.DomainDriven.Common;

public static class TypeExtensions
{
    public static bool IsDomainType(this Type type, Type baseDomainType)
    {
        return baseDomainType.IsAssignableFrom(type);
    }
}
