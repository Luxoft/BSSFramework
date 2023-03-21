namespace Framework.DomainDriven.ServiceModelGenerator;

public interface IFileStoreAttachmentGenerationEnvironmentBase : IGenerationEnvironmentBase
{
    IFileStoreGeneratorConfigurationBase FileStore { get; }
}
