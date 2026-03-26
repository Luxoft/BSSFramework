using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ServiceModelGenerator;

public interface IGenerationEnvironmentBase : IGenerationEnvironment,

                                              BLLCoreGenerator.IGeneratorConfigurationContainer,

                                              DTOGenerator.Server.IGeneratorConfigurationContainer;
