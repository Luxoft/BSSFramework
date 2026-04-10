using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using CommonFramework;

using Framework.BLL;
using Framework.BLL.Domain.ServiceRole.Base;
using Framework.BLL.Services;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.FileFactory;
using Framework.Core;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

using SecuritySystem;

#pragma warning disable S100 // Methods and properties should be named in camel case
namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public abstract class BLLCoreGeneratorConfigurationBase<TEnvironment> : CodeGeneratorConfiguration<TEnvironment, FileType>, IBLLCoreGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, IBLLCoreGenerationEnvironment
{
    private readonly Lazy<ReadOnlyCollection<Type>> lazyBLLDomainTypes;

    protected BLLCoreGeneratorConfigurationBase(TEnvironment environment)
        : base(environment)
    {
        this.lazyBLLDomainTypes = LazyHelper.Create(() => this.GetBLLDomainTypes().ToReadOnlyCollection());

        this.Logics = LazyInterfaceImplementHelper.CreateProxy(this.GetLogics);
    }

    public CodeTypeReference ActualRootSecurityServiceInterfaceType { get; } = typeof(IRootSecurityService).ToTypeReference();


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

        new CodeFileFactoryHeader<FileType>(FileType.BLLInterface, string.Empty, domainType => $"I{domainType!.Name}BLL");

    protected virtual ICodeFileFactoryHeader<FileType> BLLFactoryInterfaceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryInterface, string.Empty, domainType => $"I{domainType!.Name}BLLFactory");

    protected virtual ICodeFileFactoryHeader<FileType> BLLFactoryContainerInterfaceFileFactoryHeader =>

            new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryContainerInterface, string.Empty, _ => $"I{this.Environment.TargetSystemName}BLLFactoryContainer");



    private Type DefaultBLLFactoryType =>

            typeof(IDefaultBLLFactory<,>).MakeGenericType(this.Environment.PersistentDomainObjectBaseType, this.Environment.GetIdentityType());

    protected override IEnumerable<Type> GetDomainTypes()
    {
        var projectionTypes = from projectionEnvironment in this.Environment.ProjectionEnvironments

                              from type in projectionEnvironment.Assembly.Types

                              where this.IsPersistentObject(type)
                                    && this.Environment.MetadataProxyProvider.GetProxy(type).HasAttribute<ProjectionAttribute>(attr => attr.Role == ProjectionRole.Default)

                              select type;

        return base.GetDomainTypes().Concat(projectionTypes);
    }

    protected virtual IEnumerable<Type> GetBLLDomainTypes() =>

        from domainType in this.DomainTypes

        where this.Environment.MetadataProxyProvider.GetProxy(domainType).HasAttribute<BLLRoleAttribute>()

        select domainType;

    protected virtual IBLLFactoryContainerInterfaceGeneratorConfiguration GetLogics() => new BLLFactoryContainerInterfaceGeneratorConfiguration<BLLCoreGeneratorConfigurationBase<TEnvironment>>(this);

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders() =>
    [
        this.BLLContextInterfaceFileFactoryHeader,

        this.BLLInterfaceFileFactoryHeader,
        this.BLLFactoryInterfaceFileFactoryHeader,
        this.BLLFactoryContainerInterfaceFileFactoryHeader
    ];

    public CodeExpression GetSecurityCodeExpression(SecurityRule securityRule)
    {
        if (securityRule is SecurityRule.ModeSecurityRule)
        {
            return typeof(SecurityRule).ToTypeReferenceExpression().ToPropertyReference(securityRule.ToString());
        }
        else if (securityRule is DomainSecurityRule.DomainModeSecurityRule domainModeSecurityRule)
        {
            return this.GetSecurityCodeExpression(domainModeSecurityRule.Mode).ToMethodReferenceExpression("ToDomain", [domainModeSecurityRule.DomainType]).ToMethodInvokeExpression();
        }
        else if (securityRule is DomainSecurityRule.NonExpandedRolesSecurityRule)
        {
            return typeof(SecurityRole).ToTypeReferenceExpression().ToPropertyReference(securityRule.ToString());
        }
        else
        {
            var request = from securityRuleType in this.Environment.SecurityRuleTypeList

                          from prop in securityRuleType.GetProperties(BindingFlags.Static | BindingFlags.Public)

                          where prop.TryGetSecurityRule() == securityRule

                          select securityRuleType.ToTypeReferenceExpression().ToPropertyReference(prop);

            return request.Single(() => new Exception($"Security rule '{securityRule}' not found"));
        }
    }

    public CodeTypeReference BLLContextInterfaceTypeReference => this.GetCodeTypeReference(null, FileType.BLLContextInterface);

    public CodeTypeReference BLLFactoryInterfaceTypeReference => this.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface);


    public virtual Type? FilterModelType { get; }

    public virtual Type? ODataFilterModelType { get; }

    public virtual Type? ContextFilterModelType { get; }

    public virtual Type? ODataContextFilterModelType { get; }

    public virtual Type? CreateModelType { get; }

    public virtual Type? FormatModelType { get; }

    /// <inheritdoc />
    public virtual Type? ChangeModelType { get; }

    /// <inheritdoc />
    public virtual Type? MassChangeModelType { get; }

    public virtual Type? ExtendedModelType { get; }

    public virtual Type? IntegrationSaveModelType { get; }

    public virtual CodeExpression GetSecurityService(CodeExpression contextExpr) => contextExpr.ToPropertyReference(nameof(ISecurityServiceContainer<>.SecurityService));
}
#pragma warning restore S100 // Methods and properties should be named in camel case
