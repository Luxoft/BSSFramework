using Framework.CodeGeneration.BLLGenerator.Configuration;

namespace Framework.Authorization.TestGenerate;

public class BLLGeneratorConfiguration(ServerGenerationEnvironment environment) : GeneratorConfigurationBase<ServerGenerationEnvironment>(environment);
