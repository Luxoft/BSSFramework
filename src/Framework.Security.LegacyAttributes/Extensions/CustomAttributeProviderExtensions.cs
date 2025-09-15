using System.Reflection;

using CommonFramework;

using Framework.Core;

using SecuritySystem;

namespace Framework.Security;

public static class CustomAttributeProviderExtensions
{
    /// <summary>
    /// Получение операции просмотра (объекта или свойства)
    /// </summary>
    /// <param name="source">Источник</param>
    /// <param name="throwIfNull">Ошибка, если операция отсутствует</param>
    /// <returns></returns>
    public static SecurityRule? GetViewSecurityRule(this ICustomAttributeProvider source, bool throwIfNull = false)
    {
        var res = source.GetViewDomainObjectAttribute().Maybe(attr => attr.SecurityRule);

        if (res == null && throwIfNull)
        {
            throw new ArgumentException($"ViewDomainObjectAttribute not initialized for element \"{source}\"");
        }

        return res;
    }

    /// <summary>
    /// Получение операции редактирования (объекта или свойства)
    /// </summary>
    /// <param name="source">Источник</param>
    /// <param name="throwIfNull">Ошибка, если операция отсутствует</param>
    /// <returns></returns>
    public static SecurityRule? GetEditSecurityRule(this ICustomAttributeProvider source, bool throwIfNull = false)
    {
        var res = source.GetEditDomainObjectAttribute().Maybe(attr => attr.SecurityRule);

        if (res == null && throwIfNull)
        {
            throw new ArgumentException($"EditDomainObjectAttribute not initialized for element \"{source}\"");
        }

        return res;
    }

    public static ViewDomainObjectAttribute? GetViewDomainObjectAttribute(this ICustomAttributeProvider source)
    {
        return source.GetCustomAttribute<ViewDomainObjectAttribute>();
    }

    public static EditDomainObjectAttribute? GetEditDomainObjectAttribute(this ICustomAttributeProvider source)
    {
        return source.GetCustomAttribute<EditDomainObjectAttribute>();
    }

    public static DomainObjectAccessAttribute? GetDomainObjectAccessAttribute(this ICustomAttributeProvider source)
    {
        return source.GetCustomAttribute<DomainObjectAccessAttribute>();
    }

    public static DomainObjectAccessAttribute? GetDomainObjectAccessAttribute(this ICustomAttributeProvider source, bool isEdit)
    {
        return isEdit ? source.GetEditDomainObjectAttribute() : source.GetViewDomainObjectAttribute();
    }

    public static IEnumerable<DomainObjectAccessAttribute> GetDomainObjectAccessAttributes(this ICustomAttributeProvider source)
    {
        return from flag in new[] { true, false }

               let attr = source.GetDomainObjectAccessAttribute(flag)

               where attr != null

               select attr;
    }



    public static bool IsSecurity(this ICustomAttributeProvider source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetViewDomainObjectAttribute() != null
               || source.GetEditDomainObjectAttribute() != null
               || source.GetDomainObjectAccessAttribute() != null
               || source.HasAttribute<DependencySecurityAttribute>();
    }
}
