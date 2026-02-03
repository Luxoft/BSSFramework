using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;
using CommonFramework.Maybe;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Projection;
using Framework.Restriction;
using Framework.Security;
using Framework.Transfering;
using Framework.Validation;

#pragma warning disable S100 // Methods and properties should be named in camel case
namespace Framework.DomainDriven.BLLCoreGenerator;

public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    private readonly Lazy<ReadOnlyCollection<Type>> lazyBLLDomainTypes;

    protected GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.lazyBLLDomainTypes = LazyHelper.Create(() => this.GetBLLDomainTypes().ToReadOnlyCollection());

        this.Logics = LazyInterfaceImplementHelper.CreateProxy(this.GetLogics);

        this.ActualRootSecurityServiceInterfaceType = typeof(IRootSecurityService<>).MakeGenericType(this.Environment.PersistentDomainObjectBaseType).ToTypeReference();
    }

    public CodeTypeReference ActualRootSecurityServiceInterfaceType { get; }


    public virtual IBLLFactoryContainerInterfaceGeneratorConfiguration Logics { get; }

    public Type DefaultBLLFactoryContainerType => typeof(IBLLFactoryContainer<>).MakeGenericType(this.DefaultBLLFactoryType);

    public virtual Type SecurityBLLFactoryType => typeof(IDefaultSecurityBLLFactory<,>).MakeGenericType(this.Environment.PersistentDomainObjectBaseType, this.Environment.GetIdentityType());

    public Type SecurityBLLFactoryContainerType => typeof(IBLLFactoryContainer<>).MakeGenericType(this.SecurityBLLFactoryType);

    protected override string NamespacePostfix { get; } = "BLL";

    public ReadOnlyCollection<Type> BLLDomainTypes => this.lazyBLLDomainTypes.Value;

    /// <inheritdoc />
    public virtual bool UseDbUniquenessEvaluation { get; } = false;

    public virtual string IntegrationSaveMethodName { get; } = "IntegrationSave";

    protected virtual ICodeFileFactoryHeader<FileType> BLLContextInterfaceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.BLLContextInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}BLLContext");

    protected virtual ICodeFileFactoryHeader<FileType> BLLInterfaceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.BLLInterface, string.Empty, domainType => $"I{domainType.Name}BLL");

    protected virtual ICodeFileFactoryHeader<FileType> BLLFactoryInterfaceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryInterface, string.Empty, domainType => $"I{domainType.Name}BLLFactory");

    protected virtual ICodeFileFactoryHeader<FileType> BLLFactoryContainerInterfaceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryContainerInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}BLLFactoryContainer");



    private Type DefaultBLLFactoryType =>

            typeof(IDefaultBLLFactory<,>).MakeGenericType(this.Environment.PersistentDomainObjectBaseType, this.Environment.GetIdentityType());

    protected override IEnumerable<Type> GetDomainTypes()
    {
        var projectionTypes = from projectionEnvironment in this.Environment.ProjectionEnvironments

                              from type in projectionEnvironment.Assembly.GetTypes()

                              where this.IsPersistentObject(type)
                                    && this.Environment.ExtendedMetadata.HasAttribute<ProjectionAttribute>(
                                        type,
                                        attr => attr.Role == ProjectionRole.Default)

                              select type;

        return base.GetDomainTypes().Concat(projectionTypes);
    }

    protected virtual IEnumerable<Type> GetBLLDomainTypes()
    {
        return from domainType in this.DomainTypes

               where this.Environment.ExtendedMetadata.HasAttribute<BLLRoleAttribute>(domainType)

               select domainType;
    }

    protected virtual IBLLFactoryContainerInterfaceGeneratorConfiguration GetLogics()
    {
        return new BLLFactoryContainerInterfaceGeneratorConfiguration<GeneratorConfigurationBase<TEnvironment>>(this);
    }

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        return new[]
               {
                       this.BLLContextInterfaceFileFactoryHeader,

                       this.BLLInterfaceFileFactoryHeader,
                       this.BLLFactoryInterfaceFileFactoryHeader,
                       this.BLLFactoryContainerInterfaceFileFactoryHeader
               };
    }

    public CodeTypeReference BLLContextInterfaceTypeReference => this.GetCodeTypeReference(null, FileType.BLLContextInterface);

    public CodeTypeReference BLLFactoryInterfaceTypeReference => this.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface);


    public virtual Type FilterModelType { get; }

    public virtual Type ODataFilterModelType { get; }

    public virtual Type ContextFilterModelType { get; }

    public virtual Type ODataContextFilterModelType { get; }

    public virtual Type CreateModelType { get; }

    public virtual Type FormatModelType { get; }

    /// <inheritdoc />
    public virtual Type ChangeModelType { get; }

    /// <inheritdoc />
    public virtual Type MassChangeModelType { get; }

    public virtual Type ExtendedModelType { get; }

    public virtual Type IntegrationSaveModelType { get; }

    public virtual CodeExpression GetSecurityService(CodeExpression contextExpr)
    {
        return contextExpr.ToPropertyReference((ISecurityServiceContainer<object> container) => container.SecurityService);
    }

    public CodeExpression GetCreateDefaultBLLExpression(CodeExpression contextExpression, CodeTypeReference genericType)
    {
        return contextExpression.ToPropertyReference((IBLLFactoryContainerContext<object> context) => context.Logics)
                                .ToPropertyReference((IBLLFactoryContainer<Ignore> container) => container.Default)
                                .ToMethodReferenceExpression("Create", genericType).ToMethodInvokeExpression();
    }
}
#pragma warning restore S100 // Methods and properties should be named in camel case
