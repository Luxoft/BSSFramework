using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.Map;
using Framework.CodeGeneration.GeneratePolicy;

namespace Framework.CodeGeneration.DTOGenerator.Configuration;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, BaseFileType>
    where TEnvironment : IGenerationEnvironmentBase;

public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<BaseFileType>
{
    IGeneratePolicy<RoleFileType> GeneratePolicy { get; }

    ICodeTypeReferenceService DefaultCodeTypeReferenceService { get; }

    IReadOnlyCollection<Type> ProjectionTypes { get; }

    bool ExpandStrictMaybeToDefault { get; }

    string DataContractNamespace { get; }

    bool IdentityIsReference { get; }

    Type CollectionType { get; }

    Type ClientEditCollectionType { get; }

    string DTOIdentityPropertyName { get; }

    string DTOEmptyPropertyName { get; }

    IEnumerable<MainDTOFileType> GetNestedTypes(MainDTOFileType fileType);


    bool ForceGenerateProperties(Type domainType, DTOFileType fileType);


    IEnumerable<PropertyInfo> GetDomainTypeProperties(Type domainType, DTOFileType fileType);

    IEnumerable<PropertyInfo> GetDomainTypeProperties(Type domainType, DTOFileType fileType, bool isWritable);


    IEnumerable<GenerateTypeMap> GetTypeMaps();

    ILayerCodeTypeReferenceService? GetLayerCodeTypeReferenceService(DTOFileType fileType);

    CodeExpression GetDefaultClientDTOMappingServiceExpression();

    CodeExpression GetCreateUpdateDTOExpression(
        Type domainType,
        CodeExpression currentStrictSource,
        CodeExpression baseStrictSource,
        CodeExpression mappingService);

    //GenerateTypeMap GetTypeMap(Type domainType, DTOFileType fileType);
}
