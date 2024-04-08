using Framework.DomainDriven.ServiceModelGenerator.MethodGenerators.FileStore;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class FileStoreAttachmentGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, IFileStoreAttachmentGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IFileStoreAttachmentGenerationEnvironmentBase
{
    protected FileStoreAttachmentGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
    }

    public override string ImplementClassName { get; } = "FileStoreAttachmentFacade";


    protected override string NamespacePostfix { get; } = "ServiceFacade.FileStoreAttachments";

    public virtual string WebGetPath { get; } = "AttachmentService";

    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        yield return new GetAttachmentWebGetMethodGenerator<FileStoreAttachmentGeneratorConfigurationBase<TEnvironment>>(this, domainType);
    }

    public abstract string FileItemToMemoryStreamContextServiceName { get; }

    public Type FileItemType => this.Environment.FileStore.FileItemType;

    public Type FileItemContainerLinkType => this.Environment.FileStore.FileItemContainerLinkType;

    public Type ObjectFileContainerType => this.Environment.FileStore.ObjectFileContainerType;

    protected virtual IEnumerable<FileStoreBLLParameter> GetCustomAttributes()
    {
        yield break;
    }

    public SecurityRule TryGetSecurityAttribute(Type domainType, bool isEdit)
    {
        return this.Environment.FileStore.TryGetSecurityAttribute(domainType, isEdit);
    }

    public class FileStoreBLLParameter
    {
        public FileStoreBLLParameter(Type type, Enum securityRule, bool isEdit)
        {
            this.ForEdit = isEdit;
            this.SecurityRule = securityRule;
            this.Type = type;
        }

        public Type Type { get; }
        public Enum SecurityRule { get; }
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
