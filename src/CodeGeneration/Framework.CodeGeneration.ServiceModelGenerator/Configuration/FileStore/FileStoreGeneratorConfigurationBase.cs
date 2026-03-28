using CommonFramework;

using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.FileStore;

using SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;

public abstract class FileStoreGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, IFileStoreGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    private readonly Lazy<ILookup<Type, FileStoreBLLParameter>> dictionaryLazy;

    protected FileStoreGeneratorConfigurationBase(TEnvironment environment)
            : base(environment) =>
        this.dictionaryLazy = new Lazy<ILookup<Type, FileStoreBLLParameter>>(() => this.GetCustomAttributes().ToLookup(q => q.Type, q => q), true);

    public override string ImplementClassName { get; } = "FileStoreFacade";

    public abstract Type FileItemType { get; }

    public abstract Type FileItemContainerLinkType { get; }

    public abstract Type ObjectFileContainerType { get; }

    public abstract string AddFileItemMethodName { get; }

    public abstract string RemoveFileItemMethodName { get; }

    public abstract string GetForObjectMethodName { get; }

    protected override string NamespacePostfix { get; } = "ServiceFacade.FileStore";

    public SecurityRule? TryGetSecurityAttribute(Type type, bool isEdit)
    {
        var dict = this.dictionaryLazy.Value;
        var collection = dict[type];

        return collection.FirstOrDefault(z => z.IsEdit == isEdit).Maybe(z => z.SecurityRule);
    }


    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        yield return new GetFileContainerMethodGenerator<FileStoreGeneratorConfigurationBase<TEnvironment>>(this, this.ObjectFileContainerType, domainType);

        yield return new AddAttachmentMethodGenerator<FileStoreGeneratorConfigurationBase<TEnvironment>>(this, domainType);

        yield return new RemoveAttachmentMethodGenerator<FileStoreGeneratorConfigurationBase<TEnvironment>>(this, domainType);
    }

    protected virtual IEnumerable<FileStoreBLLParameter> GetCustomAttributes()
    {
        yield break;
    }

    public record FileStoreBLLParameter(Type Type, SecurityRule SecurityRule, bool IsEdit);
}
