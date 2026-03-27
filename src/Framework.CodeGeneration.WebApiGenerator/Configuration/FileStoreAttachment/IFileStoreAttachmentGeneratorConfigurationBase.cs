using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

using SecuritySystem;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration.FileStoreAttachment;

public interface IFileStoreAttachmentGeneratorConfigurationBase<out TEnvironment> : IFileStoreAttachmentGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IFileStoreAttachmentGenerationEnvironmentBase
{

}

public interface IFileStoreAttachmentGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    string FileItemToMemoryStreamContextServiceName { get; }

    Type FileItemType { get; }

    Type FileItemContainerLinkType { get; }

    Type ObjectFileContainerType { get; }

    string WebGetPath { get; }

    SecurityRule TryGetSecurityAttribute(Type type, bool forEdit);
}
