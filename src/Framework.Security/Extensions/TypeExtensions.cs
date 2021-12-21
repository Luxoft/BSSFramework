using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Framework.Core;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.Security
{
    public static class TypeExtensions
    {
        public static Type GetDependencySecuritySourceType(this Type type, bool recurse)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var attr = type.GetCustomAttribute<DependencySecurityAttribute>();

            if (attr != null)
            {
                if (recurse)
                {
                    return attr.SourceType.GetDependencySecuritySourceType(true) ?? attr.SourceType;
                }

                return attr.SourceType;
            }

            return null;
        }

        public static bool HasSecurityNodeInterfaces([NotNull] this Type sourceType)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            return sourceType.GetSecurityNodeInterfaces().Any();
        }

        public static IEnumerable<Type> GetSecurityNodeInterfaces([NotNull] this Type sourceType)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            return sourceType.GetAllInterfaces().Where(i => (i.IsGenericType ? i.GetGenericTypeDefinition() : i).HasAttribute<SecurityNodeAttribute>() || i == typeof(ISecurityContext));
        }

        public static IEnumerable<Type> GetGenericSecurityNodeInterfaces([NotNull] this Type sourceType)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            return sourceType.GetSecurityNodeInterfaces().Where(interfaceType => interfaceType.IsGenericType);
        }

        public static IEnumerable<Enum> GetSecurityOperationCodes(this Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));

            return from Enum securityOperationCode in Enum.GetValues(enumType)

                   //where !securityOperationCode.IsDefaultEnumValue()

                   select securityOperationCode;
        }

        public static IEnumerable<Type> GetSecurityOperationTypes(this Type type)
        {
            yield return type;

            foreach (var attr in type.GetCustomAttributes<BaseSecurityOperationTypeAttribute>())
            {
                foreach (var baseType in attr.BaseSecurityOperationType.GetSecurityOperationTypes())
                {
                    yield return baseType;
                }
            }
        }

        public static Dictionary<Type, ReadOnlyCollection<Enum>> GetTypesWithSecondarySecurityOperations([NotNull] this IEnumerable<Type> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var request = from type in source

                          let attr = type.GetViewDomainObjectAttribute()

                          where attr != null && attr.SecondaryOperations.Any()

                          select new
                          {
                              Type = type,

                              Operations = attr.AllOperations.ToReadOnlyCollection()
                          };

            return request.ToDictionary(pair => pair.Type, pair => pair.Operations);
        }
    }
}
