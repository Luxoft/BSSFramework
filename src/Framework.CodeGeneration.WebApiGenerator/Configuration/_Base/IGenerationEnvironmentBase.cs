using Framework.CodeGeneration.DomainMetadata;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

public interface IGenerationEnvironmentBase : IGenerationEnvironment,

                                              BLLCoreGenerator.Configuration.IGeneratorConfigurationContainer,

                                              DTOGenerator.Server.Configuration.IGeneratorConfigurationContainer;
