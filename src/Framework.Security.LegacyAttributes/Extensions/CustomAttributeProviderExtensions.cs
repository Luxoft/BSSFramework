using System.Reflection;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Security;

public static class CustomAttributeProviderExtensions
{
    /// <summary>
    /// Получение операции просмотра (объекта или свойства)
    /// </summary>
    /// <param name="source">Источник</param>
    /// <param name="throwIfNull">Ошибка, если операция отсутствует</param>
    /// <returns></returns>
    public static SecurityOperation GetViewSecurityOperation(this ICustomAttributeProvider source, bool throwIfNull = false)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var res = source.GetViewDomainObjectAttribute().Maybe(attr => attr.SecurityOperation);

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
    public static SecurityOperation GetEditSecurityOperation(this ICustomAttributeProvider source, bool throwIfNull = false)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var res = source.GetEditDomainObjectAttribute().Maybe(attr => attr.SecurityOperation);

        if (res == null && throwIfNull)
        {
            throw new ArgumentException($"EditDomainObjectAttribute not initialized for element \"{source}\"");
        }

        return res;
    }

    public static ViewDomainObjectAttribute GetViewDomainObjectAttribute(this ICustomAttributeProvider source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetCustomAttribute<ViewDomainObjectAttribute>();
    }

    public static EditDomainObjectAttribute GetEditDomainObjectAttribute(this ICustomAttributeProvider source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetCustomAttribute<EditDomainObjectAttribute>();
    }

    public static DomainObjectAccessAttribute GetDomainObjectAccessAttribute(this ICustomAttributeProvider source)
    {
        return source.GetCustomAttribute<DomainObjectAccessAttribute>();
    }

    public static DomainObjectAccessAttribute GetDomainObjectAccessAttribute(this ICustomAttributeProvider source, bool isEdit)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return isEdit ? (DomainObjectAccessAttribute)source.GetEditDomainObjectAttribute()
                       : (DomainObjectAccessAttribute)source.GetViewDomainObjectAttribute();
    }

    public static IEnumerable<DomainObjectAccessAttribute> GetDomainObjectAccessAttributes(this ICustomAttributeProvider source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new[] {true, false}.Select(source.GetDomainObjectAccessAttribute).Where(attr => attr != null);
    }



    public static bool IsSecurity(this ICustomAttributeProvider source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetViewDomainObjectAttribute() != null
               || source.GetEditDomainObjectAttribute() != null
               || source.GetDomainObjectAccessAttribute() != null
               || source.HasAttribute<CustomContextSecurityAttribute>()
               || source.HasAttribute<DependencySecurityAttribute>();
    }
}
