using System.CodeDom;
using System.Reflection;

using Anch.Core;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.Core;
using Framework.Database.Attributes;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Configuration;

public static class DTOGeneratorConfigurationExtensions
{
    public static bool IsReferenceProperty(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration, PropertyInfo property)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (property is null) throw new ArgumentNullException(nameof(property));

        return configuration.DomainTypes.Contains(property.PropertyType);
    }

    public static bool IsCollectionProperty(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration, PropertyInfo property)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (property is null) throw new ArgumentNullException(nameof(property));

        return property.PropertyType.GetCollectionElementType().Maybe(configuration.DomainTypes.Contains);
    }



    public static IEnumerable<CodeAttributeDeclaration> GetKnownTypesAttributes(
        this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration,
        MainDTOFileType dtoFileType,
        Type domainType)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (domainType is null) throw new ArgumentNullException(nameof(domainType));

        return from parentType in configuration.GetNestedTypes(dtoFileType)

               where configuration.GeneratePolicy.Used(domainType, parentType)

               select configuration.GetCodeTypeReference(domainType, parentType).ToKnownTypeCodeAttributeDeclaration();
    }

    public static bool IsIdentityOrVersionProperty(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration, PropertyInfo property)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (property is null) throw new ArgumentNullException(nameof(property));

        return configuration.IsIdentityProperty(property) || property.HasAttribute<VersionAttribute>();
    }

    public static CodeExpression GetIdentityPropertyCodeExpression(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration, CodeExpression? currentExpr = null)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        return (currentExpr ?? new CodeThisReferenceExpression()).ToPropertyReference(configuration.Environment.IdentityProperty);
    }

    public static CodeTypeReference GetBaseAbstractReference(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetCodeTypeReference(configuration.Environment.DomainObjectBaseType, BaseFileType.BaseAbstractDTO);
    }

    public static CodeTypeReference GetBasePersistentReference(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetCodeTypeReference(configuration.Environment.PersistentDomainObjectBaseType, BaseFileType.BasePersistentDTO);
    }

    public static CodeTypeReference GetBaseAuditPersistentReference(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetCodeTypeReference(configuration.Environment.AuditPersistentDomainObjectBaseType, BaseFileType.BaseAuditPersistentDTO);
    }

    public static CodeTypeReference GetCodeTypeReference(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration, Type domainType, MainDTOType dtoType)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (domainType is null) throw new ArgumentNullException(nameof(domainType));
        if (!Enum.IsDefined(typeof(MainDTOType), dtoType)) throw new ArgumentOutOfRangeException(nameof(dtoType));

        return configuration.GetCodeTypeReference(domainType, dtoType);
    }

    public static CodeTypeReference GetCodeTypeReference(this IDTOGeneratorConfiguration<IDTOGenerationEnvironment> configuration, Type domainType, DTOType dtoType)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (domainType is null) throw new ArgumentNullException(nameof(domainType));
        if (!Enum.IsDefined(typeof(MainDTOType), dtoType)) throw new ArgumentOutOfRangeException(nameof(dtoType));

        return configuration.GetCodeTypeReference(domainType, dtoType);
    }
}

