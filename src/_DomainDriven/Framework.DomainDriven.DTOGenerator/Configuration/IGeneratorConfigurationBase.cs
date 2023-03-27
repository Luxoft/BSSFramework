﻿using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironmentBase
{
}

public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    IGeneratePolicy<RoleFileType> GeneratePolicy { get; }

    ICodeTypeReferenceService DefaultCodeTypeReferenceService { get; }

    IReadOnlyCollection<Type> ProjectionTypes { get; }

    IReadOnlyDictionary<Type, ReadOnlyCollection<Enum>> TypesWithSecondarySecurityOperations { get; }

    bool ExpandStrictMaybeToDefault { get; }

    string DataContractNamespace { get; }

    bool IdentityIsReference { get; }

    Type CollectionType { get; }

    Type ClientEditCollectionType { get; }

    string DTOIdentityPropertyName { get; }

    string DTOEmptyPropertyName { get; }


    bool ForceGenerateProperties(Type domainType, DTOFileType fileType);


    IEnumerable<PropertyInfo> GetDomainTypeProperties([NotNull] Type domainType, [NotNull] DTOFileType fileType);

    IEnumerable<PropertyInfo> GetDomainTypeProperties(Type domainType, DTOFileType fileType, bool isWritable);


    IEnumerable<GenerateTypeMap> GetTypeMaps();

    ILayerCodeTypeReferenceService GetLayerCodeTypeReferenceService(DTOFileType fileType);

    CodeExpression GetDefaultClientDTOMappingServiceExpression();

    CodeExpression GetCreateUpdateDTOExpression(
            Type domainType,
            CodeExpression currentStrictSource,
            CodeExpression baseStrictSource,
            CodeExpression mappingService);

    //GenerateTypeMap GetTypeMap([NotNull] Type domainType, [NotNull] DTOFileType fileType);
}
