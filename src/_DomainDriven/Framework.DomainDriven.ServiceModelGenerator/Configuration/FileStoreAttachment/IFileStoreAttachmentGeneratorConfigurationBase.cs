namespace Framework.DomainDriven.ServiceModelGenerator;

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

    Enum TryGetSecurityAttribute(Type type, bool forEdit);
}
