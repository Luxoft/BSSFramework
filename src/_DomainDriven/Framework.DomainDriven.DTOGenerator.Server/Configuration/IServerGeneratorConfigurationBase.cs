using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator.Server;

public interface IServerGeneratorConfigurationBase<out TEnvironmentBase> : IServerGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironmentBase>
        where TEnvironmentBase : IServerGenerationEnvironmentBase
{
}

public interface IServerGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    IPropertyAssignerConfigurator PropertyAssignerConfigurator { get; }



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


    CodeMethodReferenceExpression GetConvertToDTOMethod(Type domainType, FileType fileType);

    CodeMethodReferenceExpression GetConvertToDTOListMethod(Type domainType, FileType fileType);

    IEnumerable<Type> GetDomainTypeMasters(Type domainType, DTOFileType fileType, bool isWritable);


    bool CanCreateDomainObject(PropertyInfo property, Type elementType, DTOFileType fileType);

    Type GetAllowCreateAttributeType(DTOFileType fileType);

    CodeAttributeDeclaration GetDTOFileAttribute(Type domainType, RoleFileType fileType);
}
