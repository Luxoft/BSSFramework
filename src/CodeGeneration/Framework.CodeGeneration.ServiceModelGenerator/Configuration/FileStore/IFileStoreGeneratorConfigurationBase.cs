using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;

using SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;

public interface IFileStoreGeneratorConfigurationBase<out TEnvironment> : IFileStoreGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
    where TEnvironment : IGenerationEnvironmentBase;

public interface IFileStoreGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    Type FileItemType { get; }

    Type FileItemContainerLinkType { get; }

    Type ObjectFileContainerType { get; }

    string AddFileItemMethodName { get; }

    string RemoveFileItemMethodName { get; }

    string GetForObjectMethodName { get; }


    SecurityRule? TryGetSecurityAttribute(Type type, bool isEdit);
}
