using Framework.CodeGeneration.Configuration;

using SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;

public interface IFileStoreGeneratorConfiguration<out TEnvironment> : IFileStoreGeneratorConfiguration, IServiceModelGeneratorConfiguration<TEnvironment>
    where TEnvironment : IServiceModelGenerationEnvironment;

public interface IFileStoreGeneratorConfiguration : ICodeGeneratorConfiguration
{
    Type FileItemType { get; }

    Type FileItemContainerLinkType { get; }

    Type ObjectFileContainerType { get; }

    string AddFileItemMethodName { get; }

    string RemoveFileItemMethodName { get; }

    string GetForObjectMethodName { get; }


    SecurityRule? TryGetSecurityAttribute(Type type, bool isEdit);
}
