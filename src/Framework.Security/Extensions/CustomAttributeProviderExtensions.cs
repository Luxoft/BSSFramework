using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;

namespace Framework.Security
{
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        /// Получение операции просмотра (объекта или свойства)
        /// </summary>
        /// <param name="source">Источник</param>
        /// <param name="throwIfNull">Ошибка, если операция отсутствует</param>
        /// <returns></returns>
        public static Enum GetViewDomainObjectCode(this ICustomAttributeProvider source, bool throwIfNull = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var res = source.GetViewDomainObjectAttribute().Maybe(attr => attr.SecurityOperationCode);

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
        public static Enum GetEditDomainObjectCode(this ICustomAttributeProvider source, bool throwIfNull = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var res = source.GetEditDomainObjectAttribute().Maybe(attr => attr.SecurityOperationCode);

            if (res == null && throwIfNull)
            {
                throw new ArgumentException($"EditDomainObjectAttribute not initialized for element \"{source}\"");
            }

            return res;
        }

        public static Enum GetDomainObjectCode(this ICustomAttributeProvider source, bool isEdit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return isEdit ? source.GetEditDomainObjectCode()
                          : source.GetViewDomainObjectCode();
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


        public static SecurityOperationAttribute GetSecurityOperationAttribute(this Enum value, bool handleDisabled = true)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return handleDisabled && value.IsDefaultEnumValue()
                 ? new SecurityOperationAttribute("Disabled", false, new Guid().ToString())
                 : value.ToFieldInfo().GetSecurityOperationAttribute();
        }

        public static SecurityOperationAttribute GetSecurityOperationAttribute(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            return fieldInfo.GetCustomAttribute<SecurityOperationAttribute>()
                .FromMaybe(() => $"SecurityOperationAttribute for field \"{fieldInfo.Name}\" not found");
        }
    }
}
