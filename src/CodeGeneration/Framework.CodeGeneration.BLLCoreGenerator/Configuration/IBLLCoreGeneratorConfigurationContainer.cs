namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public interface IBLLCoreGeneratorConfigurationContainer
{
    IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment> BLLCore { get; }
}
