using CommonFramework;

namespace Framework.CodeGeneration.Extensions;

public static class TypeExtensions
{
    public static string GetNamespacePrefix(this Type persistentDomainObjectBaseType)
    {
        if (persistentDomainObjectBaseType == null) throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));

        const string postfix = ".Domain";
        var @namespace = persistentDomainObjectBaseType.Namespace ?? string.Empty;

        return @namespace.SkipLast(postfix, false);
    }
}
