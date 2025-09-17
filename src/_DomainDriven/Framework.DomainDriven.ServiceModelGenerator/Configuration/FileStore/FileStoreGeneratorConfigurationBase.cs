using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.ServiceModelGenerator.MethodGenerators.FileStore;
using SecuritySystem;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class FileStoreGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, IFileStoreGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    private readonly Lazy<ILookup<Type, FileStoreBLLParameter>> _dictionaryLazy;

    protected FileStoreGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this._dictionaryLazy = new Lazy<ILookup<Type, FileStoreBLLParameter>>(() => this.GetCustomAttributes().ToLookup(q => q.Type, q => q), true);
    }

    public override string ImplementClassName { get; } = "FileStoreFacade";

    public abstract Type FileItemType { get; }

    public abstract Type FileItemContainerLinkType { get; }

    public abstract Type ObjectFileContainerType { get; }

    public abstract string AddFileItemMethodName { get; }

    public abstract string RemoveFileItemMethodName { get; }

    public abstract string GetForObjectMethodName { get; }

    protected override string NamespacePostfix { get; } = "ServiceFacade.FileStore";

    public virtual string WebGetPath { get; } = "AttachmentService";

    public SecurityRule TryGetSecurityAttribute(Type type, bool forEdit)
    {
        var dict = this._dictionaryLazy.Value;
        var collection = dict[type];

        return collection.FirstOrDefault(z => z.ForEdit == forEdit).Maybe(z => z.SecurityRule);
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

    public class FileStoreBLLParameter
    {
        public FileStoreBLLParameter(Type type, SecurityRule securityRule, bool isEdit)
        {
            this.ForEdit = isEdit;
            this.SecurityRule = securityRule;
            this.Type = type;
        }

        public Type Type { get; }

        public SecurityRule SecurityRule { get; }

        public bool ForEdit { get; }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() ^ this.SecurityRule.GetHashCode() ^ this.ForEdit.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var target = obj as FileStoreBLLParameter;
            if (null == target)
            {
                return false;
            }

            return this.Type == target.Type && this.SecurityRule.Equals(target.SecurityRule)
                                            && this.ForEdit.Equals(target.ForEdit);
        }
    }
}
