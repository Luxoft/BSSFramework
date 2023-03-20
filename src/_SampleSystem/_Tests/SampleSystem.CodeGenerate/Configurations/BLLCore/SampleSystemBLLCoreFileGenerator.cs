using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace SampleSystem.CodeGenerate;

/// <summary>
/// Кастомный генератор BLL-core (пример с подменой стандартного BLLInterfaceFileFactory)
/// </summary>
public class SampleSystemBLLCoreFileGenerator : BLLCoreFileGenerator<BLLCoreGeneratorConfiguration>
{
    public SampleSystemBLLCoreFileGenerator(BLLCoreGeneratorConfiguration configuration) : base(configuration)
    {
    }

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
