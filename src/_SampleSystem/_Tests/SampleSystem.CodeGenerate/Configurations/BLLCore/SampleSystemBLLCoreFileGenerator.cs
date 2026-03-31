using Framework.CodeGeneration;
using Framework.CodeGeneration.BLLCoreGenerator;
using Framework.CodeGeneration.FileFactory;

namespace SampleSystem.CodeGenerate;

/// <summary>
/// Кастомный генератор BLL-core (пример с подменой стандартного BLLInterfaceFileFactory)
/// </summary>
public class SampleSystemBLLCoreFileGenerator(BLLCoreGeneratorConfiguration configuration) : BLLCoreFileGenerator<BLLCoreGeneratorConfiguration>(configuration)
{
    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        foreach (var fileGenerator in base.GetInternalFileGenerators())
        {
            if (fileGenerator is ICodeFileFactory<FileType> bllInterfaceFileFactory && bllInterfaceFileFactory.FileType == FileType.BLLInterface)
            {
                yield return new SampleSystemBLLInterfaceFileFactory(this.Configuration, bllInterfaceFileFactory.DomainType);
            }
            else
            {
                yield return fileGenerator;
            }
        }
    }
}
