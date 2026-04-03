using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.Application.Events;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

namespace Framework.CodeGeneration.DTOGenerator.Server.Configuration;

public interface IServerDTOGeneratorConfiguration<out TEnvironmentBase> : IServerDTOGeneratorConfiguration, IDTOGeneratorConfiguration<TEnvironmentBase>
    where TEnvironmentBase : IServerDTOGenerationEnvironment;

public interface IServerDTOGeneratorConfiguration : IDTOGeneratorConfiguration
{
    bool UseRemoveMappingExtension { get; }

    IPropertyAssignerConfigurator PropertyAssignerConfigurator { get; }

    IDomainObjectEventMetadata DomainObjectEventMetadata { get; }

    Type ExceptionType { get; }

    ClientDTORole MapToDomainRole { get; }

    string ToDomainObjectMethodName { get; }

    string MapToDomainObjectMethodName { get; }



    CodeTypeReference DTOMappingServiceInterfaceTypeReference { get; }

    CodeTypeReference BLLContextTypeReference { get; }

    ReadOnlyCollection<DTOFileType> LambdaConvertTypes { get; }

    string EventDataContractNamespace { get; }

    string IntegrationDataContractNamespace { get; }

    bool CheckVersion { get; }

    Type VersionType { get; }

    PropertyInfo VersionProperty { get; }


    CodeMethodReferenceExpression GetConvertToDTOMethod(Type domainType, BaseFileType fileType);

    CodeMethodReferenceExpression GetConvertToDTOListMethod(Type domainType, BaseFileType fileType);

    IEnumerable<Type> GetDomainTypeMasters(Type domainType, DTOFileType fileType, bool isWritable);


    bool CanCreateDomainObject(PropertyInfo property, Type elementType, DTOFileType fileType);

    Type? TryGetAllowCreateAttributeType(DTOFileType fileType);

    CodeAttributeDeclaration GetDTOFileAttribute(Type domainType, RoleFileType fileType);
}
