using System.Reflection;

using Framework.Core;
using Framework.Projection;

namespace Framework.FileGeneration.Configuration;

public static class DomainMetadataBaseExtensions
{
    public static Type GetIdentityType(this IDomainMetadata domainMetadata)
    {
        if (domainMetadata is null) throw new ArgumentNullException(nameof(domainMetadata));

        return domainMetadata.IdentityProperty.PropertyType;
    }

    public static IEnumerable<Type> GetDefaultDomainTypes(this IDomainMetadata domainMetadata, bool onlyPersistent = true)
    {
        if (domainMetadata is null) throw new ArgumentNullException(nameof(domainMetadata));

        return from assembly in domainMetadata.DomainObjectAssemblies

               from type in assembly.GetTypes()

               where !type.IsAbstract

                     && (onlyPersistent ? domainMetadata.PersistentDomainObjectBaseType : domainMetadata.DomainObjectBaseType).IsAssignableFrom(type)

               orderby type.FullName

               select type;
    }

    public static IEnumerable<Type> GetModelTypes(this IDomainMetadata domainMetadata, Type domainType, Type? modelType)
    {
        if (domainType is null) throw new ArgumentNullException(nameof(domainType));

        if (modelType is null || domainType.IsProjection())
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

    public static bool IsDomainObjectBaseProperty(this IDomainMetadata domainMetadata, PropertyInfo prop)
    {
        if (domainMetadata is null) throw new ArgumentNullException(nameof(domainMetadata));
        if (prop is null) throw new ArgumentNullException(nameof(prop));

        var declareType = prop.GetTopDeclaringType();

        return declareType.IsAssignableFrom(domainMetadata.DomainObjectBaseType);
    }

    public static bool IsPersistentDomainObjectBaseProperty(this IDomainMetadata domainMetadata, PropertyInfo prop)
    {
        if (domainMetadata is null) throw new ArgumentNullException(nameof(domainMetadata));
        if (prop is null) throw new ArgumentNullException(nameof(prop));

        if (domainMetadata.IsDomainObjectBaseProperty(prop))
        {
            return false;
        }

        var declareType = prop.GetTopDeclaringType();

        return declareType.IsAssignableFrom(domainMetadata.PersistentDomainObjectBaseType);
    }

    public static bool IsAuditPersistentDomainObjectBaseProperty(this IDomainMetadata domainMetadata, PropertyInfo prop)
    {
        if (domainMetadata is null) throw new ArgumentNullException(nameof(domainMetadata));
        if (prop is null) throw new ArgumentNullException(nameof(prop));

        if (domainMetadata.IsDomainObjectBaseProperty(prop) || domainMetadata.IsPersistentDomainObjectBaseProperty(prop))
        {
            return false;
        }

        var declareType = prop.GetTopDeclaringType();

        return declareType.IsAssignableFrom(domainMetadata.AuditPersistentDomainObjectBaseType);
    }

    public static Type GetProjectionBaseType(this IDomainMetadata domainMetadata, Type projectionType)
    {
        if (domainMetadata is null) throw new ArgumentNullException(nameof(domainMetadata));
        if (projectionType is null) throw new ArgumentNullException(nameof(projectionType));

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

    public static bool IsIdentityProperty(this IDomainMetadata domainMetadata, PropertyInfo property)
    {
        if (domainMetadata is null) throw new ArgumentNullException(nameof(domainMetadata));
        if (property is null) throw new ArgumentNullException(nameof(property));

        return domainMetadata.IdentityProperty.IsAssignableFrom(property);
    }
}

