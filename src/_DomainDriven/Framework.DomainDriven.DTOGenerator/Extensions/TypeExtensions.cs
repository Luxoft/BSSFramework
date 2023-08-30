namespace Framework.DomainDriven.DTOGenerator;

public static class TypeExtensions
{
    public static bool IsAbstractDTO(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var isNormal = !type.IsAbstract;

        return !isNormal;
    }
}
