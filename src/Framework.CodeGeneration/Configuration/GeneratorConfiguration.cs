using System.CodeDom;
using System.Collections.ObjectModel;

using CommonFramework;
using CommonFramework.Maybe;
using Framework.BLL.Domain.Exceptions;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.Extensions;
using Framework.CodeGeneration.FileFactory;
using Framework.FileGeneration.Extensions;

namespace Framework.CodeGeneration.Configuration;

public abstract class GeneratorConfiguration<TEnvironment> : IGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, IGenerationEnvironment
{
    private readonly Lazy<ReadOnlyCollection<Type>> domainTypesLazy;

    private readonly Lazy<string?> defaultNamespaceLazy;

    protected GeneratorConfiguration(TEnvironment environment)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));

        this.Environment = environment;

        this.domainTypesLazy = LazyHelper.Create(() => this.GetDomainTypes().OrderBy(x => x.FullName).ToReadOnlyCollection());

        this.defaultNamespaceLazy = LazyHelper.Create(() =>
                                                       {
                                                           var request = from prefix in this.Environment.PersistentDomainObjectBaseType.GetNamespacePrefix().ToMaybe()
                                                                         from postfix in this.NamespacePostfix.ToMaybe()
                                                                         select prefix + "." + postfix;

                                                           return request.GetValueOrDefault();
                                                       });
    }

    public TEnvironment Environment { get; }

    public virtual string Namespace => this.defaultNamespaceLazy.Value;

    public IReadOnlyCollection<Type> DomainTypes => this.domainTypesLazy.Value;

    protected abstract string NamespacePostfix { get; }

    protected virtual IEnumerable<Type> GetDomainTypes()
    {
        return this.Environment.GetDefaultDomainTypes();
    }
}

public abstract class GeneratorConfiguration<TEnvironment, TFileType> : GeneratorConfiguration<TEnvironment>, IGeneratorConfiguration<TEnvironment, TFileType>
    where TEnvironment : class, IGenerationEnvironment
{
    private readonly Lazy<IReadOnlyDictionary<TFileType, ICodeFileFactoryHeader<TFileType>>> fileFactoryHeadersLazy;

    protected GeneratorConfiguration(TEnvironment environment)
        : base(environment)
    {
        this.fileFactoryHeadersLazy = LazyHelper.Create(() => this.GetFileFactoryHeaders().ToReadOnlyDictionaryI(header => header.Type));
    }

    public virtual Type ExceptionType { get; } = typeof(BusinessLogicException);

    public virtual CodeTypeReference GetCodeTypeReference(Type? domainType, TFileType fileType)
    {
        return new CodeTypeReference(this.GetTypeFullName(domainType, fileType)).WithGenerateInfo(domainType, fileType);
    }

    public virtual ICodeFileFactoryHeader? GetFileFactoryHeader(TFileType fileType, bool raiseIfNotFound = true)
    {
        var res = this.fileFactoryHeadersLazy.Value.GetValueOrDefault(fileType);

        if (res == null && raiseIfNotFound)
        {
            throw new Exception("FileFactoryHeader not found");
        }

        return res;
    }

    public string GetTypeName(Type? domainType, TFileType fileType)
    {
        return this.GetFileFactoryHeader(fileType).GetName(domainType);
    }

    protected string GetTypeFullName(Type? domainType, TFileType fileType)
    {
        return $"{this.Namespace}.{this.GetTypeName(domainType, fileType)}";
    }

    protected abstract IEnumerable<ICodeFileFactoryHeader<TFileType>> GetFileFactoryHeaders();
}
