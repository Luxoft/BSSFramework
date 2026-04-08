using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;

namespace Framework.Configuration.TestGenerate.Configurations;

public partial class ConfigurationGenerationEnvironment :

    IBLLCoreGenerationEnvironment,

    IbllGenerationEnvironment,

    IServerDTOGenerationEnvironment,

    IServiceModelGenerationEnvironment
{
    IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment> IBLLCoreGeneratorConfigurationContainer.BLLCore => this.BLLCore;

    IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment> IServerDTOGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;
}
