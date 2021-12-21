using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Projection;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Generation.Domain
{
    public static class DomainMetadataBaseExtensions
    {
        public static Type GetIdentityType([NotNull] this IDomainMetadataBase domainMetadata)
        {
            if (domainMetadata == null) throw new ArgumentNullException(nameof(domainMetadata));

            return domainMetadata.IdentityProperty.PropertyType;
        }

        public static IEnumerable<Type> GetDefaultDomainTypes([NotNull] this IDomainMetadata domainMetadata, bool onlyPersistent = true)
        {
            if (domainMetadata == null) throw new ArgumentNullException(nameof(domainMetadata));

            return from assembly in domainMetadata.DomainObjectAssemblies

                   from type in assembly.GetTypes()

                   where !type.IsAbstract

                      && (onlyPersistent ? domainMetadata.PersistentDomainObjectBaseType : domainMetadata.DomainObjectBaseType).IsAssignableFrom(type)

                   orderby type.FullName

                   select type;
        }

        public static IEnumerable<Type> GetModelTypes([NotNull] this IDomainMetadata domainMetadata, Type domainType, Type modelType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            if (modelType == null || domainType.IsProjection())
            {
                return Type.EmptyTypes;
            }

            return from assembly in domainMetadata.DomainObjectAssemblies

                   from type in assembly.GetTypes()

                   where !type.IsAbstract

                   where modelType.MakeGenericType(domainType).IsAssignableFrom(type)

                   orderby type.FullName

                   select type;
        }

        public static bool IsDomainObjectBaseProperty ([NotNull] this IDomainMetadata domainMetadata, PropertyInfo prop)
        {
            if (domainMetadata == null) throw new ArgumentNullException(nameof(domainMetadata));
            if (prop == null) throw new ArgumentNullException(nameof(prop));

            var declareType = prop.GetTopDeclaringType();

            return declareType.IsAssignableFrom(domainMetadata.DomainObjectBaseType);
        }

        public static bool IsPersistentDomainObjectBaseProperty([NotNull] this IDomainMetadata domainMetadata, PropertyInfo prop)
        {
            if (domainMetadata == null) throw new ArgumentNullException(nameof(domainMetadata));
            if (prop == null) throw new ArgumentNullException(nameof(prop));

            if (domainMetadata.IsDomainObjectBaseProperty(prop))
            {
                return false;
            }

            var declareType = prop.GetTopDeclaringType();

            return declareType.IsAssignableFrom(domainMetadata.PersistentDomainObjectBaseType);
        }

        public static bool IsAuditPersistentDomainObjectBaseProperty([NotNull] this IDomainMetadata domainMetadata, PropertyInfo prop)
        {
            if (domainMetadata == null) throw new ArgumentNullException(nameof(domainMetadata));
            if (prop == null) throw new ArgumentNullException(nameof(prop));

            if (domainMetadata.IsDomainObjectBaseProperty(prop) || domainMetadata.IsPersistentDomainObjectBaseProperty(prop))
            {
                return false;
            }

            var declareType = prop.GetTopDeclaringType();

            return declareType.IsAssignableFrom(domainMetadata.AuditPersistentDomainObjectBaseType);
        }

        public static Type GetProjectionBaseType([NotNull] this IDomainMetadata domainMetadata, [NotNull] Type projectionType)
        {
            if (domainMetadata == null) throw new ArgumentNullException(nameof(domainMetadata));
            if (projectionType == null) throw new ArgumentNullException(nameof(projectionType));

            if (domainMetadata.PersistentDomainObjectBaseType.IsAssignableFrom(projectionType))
            {
                return domainMetadata.PersistentDomainObjectBaseType;
            }
            else if (domainMetadata.DomainObjectBaseType.IsAssignableFrom(projectionType))
            {
                return domainMetadata.DomainObjectBaseType;
            }
            else
            {
                throw new ArgumentException("Invalid projection type", nameof(projectionType));
            }
        }

        public static bool IsIdentityProperty(this IDomainMetadata domainMetadata, [NotNull] PropertyInfo property)
        {
            if (domainMetadata == null) throw new ArgumentNullException(nameof(domainMetadata));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return domainMetadata.IdentityProperty.IsAssignableFrom(property);
        }
    }
}
