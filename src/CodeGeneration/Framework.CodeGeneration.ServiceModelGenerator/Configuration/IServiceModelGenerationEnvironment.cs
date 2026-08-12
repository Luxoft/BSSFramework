using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration;

public interface IServiceModelGenerationEnvironment : ICodeGenerationEnvironment,

    BLLCoreGenerator.Configuration.IBLLCoreGeneratorConfigurationContainer,

    DTOGenerator.Server.Configuration.IServerDTOGeneratorConfigurationContainer;
