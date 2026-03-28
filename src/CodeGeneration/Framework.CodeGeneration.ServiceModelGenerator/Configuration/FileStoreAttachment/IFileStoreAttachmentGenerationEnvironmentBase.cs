using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStoreAttachment;

public interface IFileStoreAttachmentGenerationEnvironmentBase : IGenerationEnvironmentBase
{
    IFileStoreGeneratorConfigurationBase FileStore { get; }
}
