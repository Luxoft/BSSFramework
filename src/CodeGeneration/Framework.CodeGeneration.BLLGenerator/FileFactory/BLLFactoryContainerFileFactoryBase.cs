using System.CodeDom;

using Framework.CodeGeneration.BLLGenerator.Configuration;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public abstract class BLLFactoryContainerFileFactoryBase<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
    public override FileType FileType => FileType.BLLFactoryContainer;


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.Configuration.Environment.BLLCore.GetCodeTypeReference(null, BLLCoreGenerator.FileType.BLLFactoryContainerInterface);
    }
}
