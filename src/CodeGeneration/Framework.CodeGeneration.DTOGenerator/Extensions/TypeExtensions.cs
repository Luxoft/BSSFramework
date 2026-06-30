namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class TypeExtensions
{
    public static bool IsAbstractDTO(this Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var isNormal = !type.IsAbstract;

        return !isNormal;
    }
}
