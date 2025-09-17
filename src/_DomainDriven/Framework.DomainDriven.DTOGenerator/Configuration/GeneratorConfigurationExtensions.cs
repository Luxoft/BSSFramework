using System.CodeDom;
using System.ComponentModel;
using System.Reflection;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator;

public static class GeneratorConfigurationExtensions
{
    public static bool IsReferenceProperty(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, PropertyInfo property)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (property == null) throw new ArgumentNullException(nameof(property));

        return configuration.DomainTypes.Contains(property.PropertyType);
    }

    public static bool IsCollectionProperty(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, PropertyInfo property)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.PropertyType.GetCollectionElementType().Maybe(configuration.DomainTypes.Contains);
    }



    public static IEnumerable<CodeAttributeDeclaration> GetKnownTypesAttributes(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, MainDTOFileType dtoFileType, Type domainType)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return from parentType in dtoFileType.GetNestedTypes()

               where configuration.GeneratePolicy.Used(domainType, parentType)

               select configuration.GetCodeTypeReference(domainType, parentType).ToKnownTypeCodeAttributeDeclaration();
    }

    public static bool IsIdentityOrVersionProperty(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, PropertyInfo property)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (property == null) throw new ArgumentNullException(nameof(property));

        return configuration.IsIdentityProperty(property) || property.HasAttribute<VersionAttribute>();
    }

    public static CodeExpression GetIdentityPropertyCodeExpression(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, CodeExpression currentExpr = null)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return (currentExpr ?? new CodeThisReferenceExpression()).ToPropertyReference(configuration.Environment.IdentityProperty);
    }

    public static CodeTypeReference GetBaseAbstractReference(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetCodeTypeReference(configuration.Environment.DomainObjectBaseType, FileType.BaseAbstractDTO);
    }

    public static CodeTypeReference GetBasePersistentReference(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetCodeTypeReference(configuration.Environment.PersistentDomainObjectBaseType, FileType.BasePersistentDTO);
    }

    public static CodeTypeReference GetBaseAuditPersistentReference(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetCodeTypeReference(configuration.Environment.AuditPersistentDomainObjectBaseType, FileType.BaseAuditPersistentDTO);
    }

    public static CodeTypeReference GetCodeTypeReference(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, Type domainType, MainDTOType dtoType)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (!Enum.IsDefined(typeof(MainDTOType), dtoType)) throw new InvalidEnumArgumentException(nameof(dtoType), (int)dtoType, typeof(MainDTOType));

        return configuration.GetCodeTypeReference(domainType, dtoType);
    }

    public static CodeTypeReference GetCodeTypeReference(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, Type domainType, DTOType dtoType)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (!Enum.IsDefined(typeof(DTOType), dtoType)) throw new InvalidEnumArgumentException(nameof(dtoType), (int)dtoType, typeof(DTOType));

        return configuration.GetCodeTypeReference(domainType, dtoType);
    }
}
