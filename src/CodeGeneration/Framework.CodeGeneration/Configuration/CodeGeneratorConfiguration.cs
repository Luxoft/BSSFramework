using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.Exceptions;
using Framework.CodeGeneration.Extensions;
using Framework.CodeGeneration.FileFactory;
using Framework.FileGeneration.Configuration;
using Framework.FileGeneration.Extensions;

namespace Framework.CodeGeneration.Configuration;

public abstract class CodeGeneratorConfiguration<TEnvironment> : FileGeneratorConfiguration<TEnvironment>, ICodeGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, ICodeGenerationEnvironment
{
    private readonly Lazy<string?> defaultNamespaceLazy;

    protected CodeGeneratorConfiguration(TEnvironment environment)
        :base(environment) =>
        this.defaultNamespaceLazy = LazyHelper.Create(() =>
        {
            var request = from prefix in this.Environment.PersistentDomainObjectBaseType.GetNamespacePrefix().ToMaybe()
                          from postfix in this.NamespacePostfix.ToMaybe()
                          select prefix + "." + postfix;

            return request.GetValueOrDefault();
        });

    public virtual string? Namespace => this.defaultNamespaceLazy.Value;

    protected abstract string NamespacePostfix { get; }
}

public abstract class CodeGeneratorConfiguration<TEnvironment, TFileType> : CodeGeneratorConfiguration<TEnvironment>, ICodeGeneratorConfiguration<TEnvironment, TFileType>
    where TEnvironment : class, ICodeGenerationEnvironment
    where TFileType : notnull
{
    private readonly Lazy<IReadOnlyDictionary<TFileType, ICodeFileFactoryHeader<TFileType>>> fileFactoryHeadersLazy;

    protected CodeGeneratorConfiguration(TEnvironment environment)
        : base(environment) =>
        this.fileFactoryHeadersLazy = LazyHelper.Create(() => this.GetFileFactoryHeaders().ToReadOnlyDictionaryI(header => header.Type));

    public virtual Type ExceptionType { get; } = typeof(BusinessLogicException);

    public virtual CodeTypeReference GetCodeTypeReference(Type? domainType, TFileType fileType) => new CodeTypeReference(this.GetTypeFullName(domainType, fileType)).WithGenerateInfo(domainType, fileType);

    public virtual ICodeFileFactoryHeader? GetFileFactoryHeader(TFileType fileType, bool raiseIfNotFound = true)
    {
        var res = this.fileFactoryHeadersLazy.Value.GetValueOrDefault(fileType);

        if (res == null && raiseIfNotFound)
        {
            throw new Exception("FileFactoryHeader not found");
        }

        return res;
    }

    public string GetTypeName(Type? domainType, TFileType fileType) => this.GetFileFactoryHeader(fileType)!.GetName(domainType);

    protected string GetTypeFullName(Type? domainType, TFileType fileType) => $"{this.Namespace}.{this.GetTypeName(domainType, fileType)}";

    protected abstract IEnumerable<ICodeFileFactoryHeader<TFileType>> GetFileFactoryHeaders();
}
