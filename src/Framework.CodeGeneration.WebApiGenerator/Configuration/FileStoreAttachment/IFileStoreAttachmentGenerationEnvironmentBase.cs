using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Configuration.FileStore;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration.FileStoreAttachment;

public interface IFileStoreAttachmentGenerationEnvironmentBase : IGenerationEnvironmentBase
{
    IFileStoreGeneratorConfigurationBase FileStore { get; }
}
