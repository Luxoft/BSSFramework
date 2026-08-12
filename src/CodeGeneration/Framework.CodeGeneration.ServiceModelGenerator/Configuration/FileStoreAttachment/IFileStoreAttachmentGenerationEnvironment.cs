using Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStoreAttachment;

public interface IFileStoreAttachmentGenerationEnvironment : IServiceModelGenerationEnvironment
{
    IFileStoreGeneratorConfiguration FileStore { get; }
}
