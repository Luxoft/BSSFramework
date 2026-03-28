namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerationEnvironment :

    CodeGeneration.BLLCoreGenerator.Configuration.IGenerationEnvironmentBase,

    CodeGeneration.BLLGenerator.Configuration.IGenerationEnvironmentBase,

    CodeGeneration.DTOGenerator.Server.Configuration.IServerGenerationEnvironmentBase,

    CodeGeneration.ServiceModelGenerator.Configuration._Base.IGenerationEnvironmentBase

{
    CodeGeneration.BLLCoreGenerator.Configuration.IGeneratorConfigurationBase<CodeGeneration.BLLCoreGenerator.Configuration.IGenerationEnvironmentBase>
        CodeGeneration.BLLCoreGenerator.Configuration.IGeneratorConfigurationContainer.BLLCore => this.BLLCore;

    CodeGeneration.DTOGenerator.Server.Configuration.IServerGeneratorConfigurationBase<CodeGeneration.DTOGenerator.Server.Configuration.IServerGenerationEnvironmentBase>
        CodeGeneration.DTOGenerator.Server.Configuration.IGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;
}
