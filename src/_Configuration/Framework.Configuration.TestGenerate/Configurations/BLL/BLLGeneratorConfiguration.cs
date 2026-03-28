using Framework.CodeGeneration.BLLGenerator.Configuration;

namespace Framework.Configuration.TestGenerate.Configurations.BLL;

public class BLLGeneratorConfiguration(ServerGenerationEnvironment environment) : GeneratorConfigurationBase<ServerGenerationEnvironment>(environment);
