using Framework.CodeGeneration.DomainMetadata;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;

public interface IGenerationEnvironmentBase : IGenerationEnvironment,

    BLLCoreGenerator.Configuration.IGeneratorConfigurationContainer,

    DTOGenerator.Server.Configuration.IGeneratorConfigurationContainer;
