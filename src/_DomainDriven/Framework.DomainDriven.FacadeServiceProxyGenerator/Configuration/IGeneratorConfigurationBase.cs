using System;
using System.CodeDom;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironmentBase
    {
    }

    public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>, IFacadeConfiguration
    {
        string ContractConfigurationName { get; }

        string ContractNamespace { get; }

        CodeTypeReference ResolveMethodParameterType(Type type);

        CodeAttributeDeclaration GetServiceContractAttribute();

        CodeAttributeDeclaration GetOperationContractAttribute(MethodInfo method);
    }
}
