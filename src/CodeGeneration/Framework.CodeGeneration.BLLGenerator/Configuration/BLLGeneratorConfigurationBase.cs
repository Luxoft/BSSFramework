using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Fetching;
using Framework.BLL.Fetching.PathFactory;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.FileFactory;
using Framework.Core;
using Framework.FileGeneration.Configuration;
using Framework.Restriction;
using Framework.Validation;

namespace Framework.CodeGeneration.BLLGenerator.Configuration;

public abstract class BLLGeneratorConfigurationBase<TEnvironment> : CodeGeneratorConfiguration<TEnvironment, FileType>, IbllGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, IbllGenerationEnvironment
{

    private readonly Lazy<ReadOnlyCollection<Type>> lazyValidationTypes;

    protected BLLGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.lazyValidationTypes = LazyHelper.Create(() => this.GetValidationTypes().ToReadOnlyCollection());

        this.Logics = LazyInterfaceImplementHelper.CreateProxy(this.GetLogics);

        this.FetchPathFactory = LazyInterfaceImplementHelper.CreateProxy(this.CreateFetchPathFactory);
    }

    /// <inheritdoc />
    public virtual bool GenerateExternalPropertyValidators { get; } = false;

    /// <inheritdoc />
    public virtual bool GenerateExternalClassValidators { get; } = false;

    public ReadOnlyCollection<Type> ValidationTypes => this.lazyValidationTypes.Value;


    protected virtual IEnumerable<Type> GetValidationTypes()
    {
        var baseTypes = this.Environment.GetDefaultDomainTypes(false).ToArray();

        var referencedTypes = baseTypes.GetReferencedTypes().Where(referencedType => referencedType.HasExpandValidation()).OrderBy(type => type.FullName).ToArray();

        var classes = from referencedType in referencedTypes

                      where referencedType.IsClass
                            && !referencedType.IsGenericTypeDefinition
                            && referencedType != typeof(string)
                            && referencedType != typeof(object)
                            && !referencedType.IsArray
                            && !this.Environment.DomainObjectBaseType.IsAssignableFrom(referencedType)

                      select referencedType;

        var structs = from referencedType in referencedTypes

                      where referencedType.IsValueType
                            && !referencedType.IsPrimitiveType()
                            && !referencedType.IsEnum

                      select referencedType;

        return baseTypes.Concat(classes).Concat(structs).Distinct();
    }

    /// <inheritdoc />
    public virtual bool SquashClassValidators(Type domainType) => true;

    /// <inheritdoc />
    public virtual bool SquashPropertyValidators(PropertyInfo property) => true;

    public virtual bool GenerateDomainServiceConstructor(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return true;
    }

    public virtual IbllFactoryContainerGeneratorConfiguration Logics { get; }

    public IFetchPathFactory<ViewDTOType> FetchPathFactory { get; }

    public virtual bool GenerateDTOFetchRuleExpander => true;


    public CodeTypeReference BLLContextTypeReference => this.Environment.BLLCore.BLLContextInterfaceTypeReference;

    protected override string NamespacePostfix { get; } = "BLL";

    protected virtual IFetchPathFactory<ViewDTOType> CreateFetchPathFactory()
    {
        IFetchPathFactory<ViewDTOType> factory = new ExpandFetchPathFactory(this.Environment.MetadataProxyProvider.Wrap(this.Environment.PersistentDomainObjectBaseType));

        return factory.WithCompress();
    }


    /// <inheritdoc />
    public virtual bool AllowVirtualValidation(PropertyInfo property)
    {
        var modeAttr = property.GetCustomAttribute<PropertyValidationModeAttribute>();

        if (modeAttr != null && modeAttr.HasValue(true))
        {
            return true;
        }

        return property.HasAttribute(attr => attr is PropertyValidatorAttribute || attr is IRestrictionAttribute);
    }

    public virtual IValidatorGenerator GetValidatorGenerator(Type domainType, CodeExpression validatorMapExpr)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (validatorMapExpr == null) throw new ArgumentNullException(nameof(validatorMapExpr));

        return new DefaultValidatorGenerator<IbllGeneratorConfiguration<TEnvironment>>(this, domainType, validatorMapExpr);
    }

    public virtual bool GenerateValidation { get; } = true;

    public virtual Type OperationContextType { get; } = typeof(OperationContextBase);

    public virtual bool UseDbUniquenessEvaluation { get; } = false;

    public virtual bool GenerateBllConstructor(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return true;
    }

    protected override IEnumerable<Type> GetDomainTypes() => this.Environment.BLLCore.BLLDomainTypes;

    protected virtual IbllFactoryContainerGeneratorConfiguration GetLogics() => new BLLFactoryContainerGeneratorConfiguration<BLLGeneratorConfigurationBase<TEnvironment>>(this);

    protected virtual ICodeFileFactoryHeader<FileType> SecurityDomainBLLBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.SecurityDomainBLLBase, string.Empty, _ => FileType.SecurityDomainBLLBase);

    protected virtual ICodeFileFactoryHeader<FileType> ValidatorBaseFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.ValidatorBase, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.ValidatorBase}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidatorCompileCacheFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.ValidatorCompileCache, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.ValidatorCompileCache}");

    protected virtual ICodeFileFactoryHeader<FileType> MainDTOFetchRuleExpanderFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.MainDTOFetchRuleExpander, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.MainDTOFetchRuleExpander}");

    protected virtual ICodeFileFactoryHeader<FileType> MainDTOFetchRuleExpanderBaseFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.MainDTOFetchRuleExpanderBase, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.MainDTOFetchRuleExpanderBase}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidationMapFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.ValidationMap, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.ValidationMap}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidationMapBaseFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.ValidationMapBase, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.ValidationMapBase}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidatorFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.Validator, string.Empty, _ => $"{this.Environment.TargetSystemName}{FileType.Validator}");

    protected virtual ICodeFileFactoryHeader<FileType> ValidatorInterfaceFileFactoryHeader =>

        new CodeFileFactoryHeader<FileType>(FileType.ValidatorInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}{FileType.Validator}");

    public CodeTypeReference SecurityDomainBLLBaseTypeReference => this.GetSecurityDomainBLLBaseTypeReference(null);

    public CodeTypeReference GetSecurityDomainBLLBaseTypeReference(Type? type) => this.GetCodeTypeReference(type, FileType.SecurityDomainBLLBase);

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        yield return this.ValidationMapBaseFileFactoryHeader;
        yield return this.ValidationMapFileFactoryHeader;

        yield return this.ValidatorCompileCacheFileFactoryHeader;

        yield return this.ValidatorBaseFileFactoryHeader;
        yield return this.ValidatorFileFactoryHeader;
        yield return this.ValidatorInterfaceFileFactoryHeader;

        yield return this.MainDTOFetchRuleExpanderBaseFileFactoryHeader;
        yield return this.MainDTOFetchRuleExpanderFileFactoryHeader;

        yield return this.SecurityDomainBLLBaseFileFactoryHeader;

        yield return new CodeFileFactoryHeader<FileType>(FileType.BLLContext, "", _ => this.Environment.TargetSystemName + FileType.BLLContext);

        yield return new CodeFileFactoryHeader<FileType>(FileType.BLL, "", domainType => domainType!.Name + FileType.BLL);

        yield return new CodeFileFactoryHeader<FileType>(FileType.BLLFactory, "", domainType => domainType!.Name + FileType.BLLFactory);

        yield return new CodeFileFactoryHeader<FileType>(FileType.DefaultBLLFactory, "", _ => this.Environment.TargetSystemName + FileType.DefaultBLLFactory);

        yield return new CodeFileFactoryHeader<FileType>(FileType.ImplementedBLLFactory, "", _ => this.Environment.TargetSystemName + FileType.ImplementedBLLFactory);

        yield return new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryContainer, "", _ => this.Environment.TargetSystemName + FileType.BLLFactoryContainer);
    }
}
