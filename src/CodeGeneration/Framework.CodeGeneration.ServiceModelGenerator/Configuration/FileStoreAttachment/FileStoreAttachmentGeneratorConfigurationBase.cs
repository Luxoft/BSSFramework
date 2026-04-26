using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.FileStore;

using Anch.SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStoreAttachment;

public abstract class FileStoreAttachmentGeneratorConfigurationBase<TEnvironment>(TEnvironment environment)
    : ServiceModelGeneratorBase<TEnvironment>(environment), IFileStoreAttachmentGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IFileStoreAttachmentGenerationEnvironment
{
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

    public SecurityRule TryGetSecurityAttribute(Type domainType, bool isEdit) => this.Environment.FileStore.TryGetSecurityAttribute(domainType, isEdit);

    public class FileStoreBLLParameter(Type type, Enum securityRule, bool isEdit)
    {
        public Type Type { get; } = type;

        public Enum SecurityRule { get; } = securityRule;

        public bool ForEdit { get; } = isEdit;

        public override int GetHashCode() => this.Type.GetHashCode() ^ this.SecurityRule.GetHashCode() ^ this.ForEdit.GetHashCode();

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
