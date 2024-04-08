using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModelGenerator;

public interface IFileStoreGeneratorConfigurationBase<out TEnvironment> : IFileStoreGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{

}

public interface IFileStoreGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    Type FileItemType { get; }

    Type FileItemContainerLinkType { get; }

    Type ObjectFileContainerType { get; }

    string AddFileItemMethodName { get; }

    string RemoveFileItemMethodName { get; }

    string GetForObjectMethodName { get; }



    SecurityRule TryGetSecurityAttribute(Type type, bool forEdit);
}
