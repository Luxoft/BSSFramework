using Framework.CodeGeneration.Configuration;

using SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStoreAttachment;

public interface IFileStoreAttachmentGeneratorConfiguration<out TEnvironment> : IFileStoreAttachmentGeneratorConfiguration, IServiceModelGeneratorConfiguration<TEnvironment>
        where TEnvironment : IFileStoreAttachmentGenerationEnvironment;

public interface IFileStoreAttachmentGeneratorConfiguration : ICodeGeneratorConfiguration
{
    string FileItemToMemoryStreamContextServiceName { get; }

    Type FileItemType { get; }

    Type FileItemContainerLinkType { get; }

    Type ObjectFileContainerType { get; }

    string WebGetPath { get; }

    SecurityRule TryGetSecurityAttribute(Type type, bool forEdit);
}
