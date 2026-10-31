using Framework.CodeGeneration;
using Framework.CodeGeneration.BLLCoreGenerator;
using Framework.CodeGeneration.FileFactory;

namespace SampleSystem.CodeGenerate.Configurations.BLLCore;

/// <summary>
/// Custom BLL-core generator (an example with replacing the standard BLLInterfaceFileFactory)
/// </summary>
public class SampleSystemBLLCoreFileGenerator(BLLCoreGeneratorConfiguration configuration) : BLLCoreFileGenerator<BLLCoreGeneratorConfiguration>(configuration)
{
    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        foreach (var fileGenerator in base.GetInternalFileGenerators())
        {
            if (fileGenerator is ICodeFileFactory<FileType> bllInterfaceFileFactory && bllInterfaceFileFactory.FileType == FileType.BLLInterface)
            {
                yield return new SampleSystemBLLInterfaceFileFactory(this.Configuration, bllInterfaceFileFactory.DomainType!);
            }
            else
            {
                yield return fileGenerator;
            }
        }
    }
}
