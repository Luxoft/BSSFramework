using System.CodeDom;
using System.Reflection;

using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Fetching;
using Framework.BLL.Domain.Persistent.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.ProjectionGenerator._Extensions;
using Framework.Core;
using Framework.Database.Mapping;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

namespace Framework.CodeGeneration.ProjectionGenerator.Configuration;

public abstract class ProjectionGeneratorConfigurationBase<TEnvironment> : CodeGeneratorConfiguration<TEnvironment, FileType>, IProjectionGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IProjectionGenerationEnvironment
{
    private readonly IProjectionEnvironment projectionEnvironment;

    protected ProjectionGeneratorConfigurationBase(TEnvironment environment, IProjectionEnvironment? projectionEnvironment = null)
        : base(environment) =>
        this.projectionEnvironment = projectionEnvironment
                                     ?? this.Environment.ProjectionEnvironments.SingleOrDefault()
                                     ?? throw new InvalidOperationException("ProjectionEnvironment not initialized");

    public override string Namespace => this.projectionEnvironment.Namespace;

    protected override string NamespacePostfix { get; } = "Domain.Projection";

    /// <inheritdoc />
    public virtual bool OneToOneSetter { get; } = true;

    /// <inheritdoc />
    public virtual bool GeneratePublicCtors { get; } = false;

    protected virtual ICodeFileFactoryHeader<FileType> ProjectionFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.Projection, string.Empty, domainType => domainType!.Name);

    protected virtual ICodeFileFactoryHeader<FileType> CustomProjectionBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.CustomProjectionBase, string.Empty, domainType => $"Custom{domainType!.Name}Base");


    /// <inheritdoc />
    public virtual IEnumerable<CodeAttributeDeclaration> GetDomainTypeAttributeDeclarations(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        {
            var projectionAttr = domainType.GetCustomAttribute<ProjectionAttribute>();

            if (projectionAttr != null)
            {
                yield return projectionAttr.ToAttributeDeclaration();
            }
        }


        {
            var bllProjectionViewAttr = domainType.GetCustomAttribute<BLLProjectionViewRoleAttribute>();

            if (bllProjectionViewAttr != null)
            {
                yield return bllProjectionViewAttr.ToAttributeDeclaration();
            }
        }

        {
            var dependencySecurityAttr = domainType.GetCustomAttribute<DependencySecurityAttribute>();

            if (dependencySecurityAttr != null)
            {
                yield return dependencySecurityAttr.ToAttributeDeclaration();
            }
        }

        {
            foreach (var projectionFilterAttribute in this.Environment.MetadataProxyProvider.Wrap(domainType).GetCustomAttributes<ProjectionFilterAttribute>())
            {
                yield return projectionFilterAttribute.ToAttributeDeclaration();
            }
        }

        {
            foreach (var securityAttribute in this.Environment.MetadataProxyProvider.Wrap(domainType).GetSecurityAttributes(this.Environment.SecurityRuleTypeList))
            {
                yield return securityAttribute;
            }
        }

        if (this.IsPersistentObject(domainType))
        {
            var tableAttr = domainType.GetCustomAttribute<TableAttribute>();

            if (tableAttr != null)
            {
                yield return tableAttr.ToAttributeDeclaration();
            }

            var inlineBaseTypeAttr = domainType.GetCustomAttribute<InlineBaseTypeMappingAttribute>();

            if (inlineBaseTypeAttr != null)
            {
                yield return inlineBaseTypeAttr.ToAttributeDeclaration();
            }
        }
    }

    /// <inheritdoc />
    public virtual IEnumerable<CodeAttributeDeclaration> GetPropertyAttributeDeclarations(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        foreach (var securityAttribute in this.Environment.MetadataProxyProvider.Wrap(property).GetSecurityAttributes(this.Environment.SecurityRuleTypeList))
        {
            yield return securityAttribute;
        }

        if (property.GetCustomAttribute<CustomSerializationAttribute>(attr => attr.Role.HasFlag(DTORole.Client)) is { } customSerializationAttr)
        {
            yield return customSerializationAttr.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<ProjectionPropertyAttribute>() is { } projectionPropAttr)
        {
            yield return projectionPropAttr.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<ExpandPathAttribute>() is { } expandPathAttribute)
        {
            yield return expandPathAttribute.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<MappingAttribute>() is { } mappingAttr)
        {
            yield return mappingAttr.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<IgnoreFetchAttribute>() is { } ignoreFetchAttribute)
        {
            yield return ignoreFetchAttribute.ToAttributeDeclaration();
        }

        foreach (var fetchPathAttribute in this.Environment.MetadataProxyProvider.Wrap(property).GetCustomAttributes<FetchPathAttribute>())
        {
            yield return fetchPathAttribute.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<MappingPropertyAttribute>() is { } mappingPropertyAttribute)
        {
            yield return mappingPropertyAttribute.ToAttributeDeclaration();
        }
    }

    protected override IEnumerable<Type> GetDomainTypes() => this.projectionEnvironment.Assembly.GetTypes().Where(this.Environment.DomainObjectBaseType.IsAssignableFrom);

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        yield return this.ProjectionFileFactoryHeader;
        yield return this.CustomProjectionBaseFileFactoryHeader;
    }
}
