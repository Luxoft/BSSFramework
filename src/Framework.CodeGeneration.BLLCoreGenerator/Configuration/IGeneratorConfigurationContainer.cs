namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public interface IGeneratorConfigurationContainer
{
    IGeneratorConfigurationBase<IGenerationEnvironmentBase> BLLCore { get; }
}
