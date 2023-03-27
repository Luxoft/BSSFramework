using System.CodeDom;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironmentBase
{
}

public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    IBLLFactoryContainerGeneratorConfiguration Logics { get; }

    CodeTypeReference BLLContextTypeReference { get; }

    bool GenerateBllConstructor(Type domainType);
}
