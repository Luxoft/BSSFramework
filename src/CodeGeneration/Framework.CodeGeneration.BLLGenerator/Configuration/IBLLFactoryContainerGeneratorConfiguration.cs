namespace Framework.CodeGeneration.BLLGenerator.Configuration;

public interface IBLLFactoryContainerGeneratorConfiguration
{
    IEnumerable<ICodeFile> GetFileFactories();
}
