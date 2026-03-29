namespace Framework.CodeGeneration.BLLGenerator.Configuration.BLLFactoryContainer;

public interface IBLLFactoryContainerGeneratorConfiguration
{
    IEnumerable<ICodeFile> GetFileFactories();
}
