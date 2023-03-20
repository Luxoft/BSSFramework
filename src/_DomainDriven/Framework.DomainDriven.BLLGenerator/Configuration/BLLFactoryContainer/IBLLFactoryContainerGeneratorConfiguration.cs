using System.Collections.Generic;
using Framework.DomainDriven.Generation;

namespace Framework.DomainDriven.BLLGenerator;

public interface IBLLFactoryContainerGeneratorConfiguration
{
    IEnumerable<ICodeFile> GetFileFactories();
}
