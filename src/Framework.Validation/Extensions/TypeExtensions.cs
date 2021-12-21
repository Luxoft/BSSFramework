using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Restriction;

using JetBrains.Annotations;

namespace Framework.Validation
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Проверка типа на глубокую комопзитную валидацию
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns></returns>
        public static bool HasExpandValidation([NotNull] this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.HasAttribute<ExpandValidationAttribute>();
        }

        public static IEnumerable<PropertyInfo> GetValidationProperties(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)

                   let getMethod = property.GetGetMethod()

                   where getMethod != null && !property.GetIndexParameters().Any()
                      && !property.HasAttribute<PropertyValidationModeAttribute>(attr => attr.HasValue(false))

                   orderby property.PropertyType.IsCollection()

                   select property;
        }

        internal static bool IsSystemOrCoreType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsSystemType() || type.IsCoreType();
        }

        internal static bool IsSystemType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.Assembly == typeof(object).Assembly;
        }

        internal static bool IsCoreType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.Assembly == typeof(Ignore).Assembly;
        }

        public static IEnumerable<PropertyInfo> GetUniqueElementPropeties(this Type type, string groupKey)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return from elementProperty in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)

                   let getElementPropertyMethod = elementProperty.GetGetMethod()

                   where getElementPropertyMethod != null
                      && elementProperty.HasAttribute<UniqueElementAttribute>(attr => string.Equals(groupKey, attr.Key, StringComparison.CurrentCultureIgnoreCase))

                   select elementProperty;
        }

        public static PropertyInfo[] GetUniqueElementPropeties(this Type type, string groupKey, bool checkAnyElements)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var uniProperties = type.GetUniqueElementPropeties(groupKey).ToArray();

            if (!uniProperties.Any() && checkAnyElements)
            {
                throw new Exception($"No required group elements for key \"{groupKey}\" in domainType \"{type.Name}\"");
            }

            return uniProperties;
        }

        public static PropertyInfo[] GetUniqueElementPropeties(this PropertyInfo property, string groupKey, bool checkAnyElements)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var elementType = property.PropertyType.GetCollectionElementType();

            var uniProperties = elementType.GetUniqueElementPropeties(groupKey).ToArray();

            if (!uniProperties.Any() && checkAnyElements)
            {
                throw new Exception($"No group elements for key \"{groupKey}\" for property \"{property.Name}\" in domainType \"{property.ReflectedType.Name}\"");
            }

            return uniProperties;
        }


        public static string GetValidationName(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetCustomAttribute<CustomNameAttribute>().Maybe(v => v.Name) ?? type.Name;
        }

        public static string GetValidationName(this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return property.GetCustomAttribute<CustomNameAttribute>().Maybe(v => v.Name) ?? property.Name;
        }
    }
}
