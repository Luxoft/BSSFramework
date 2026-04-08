namespace Framework.CodeGeneration.BLLGenerator.Configuration;

public interface IbllFactoryContainerGeneratorConfiguration
{
    IEnumerable<ICodeFile> GetFileFactories();
}
