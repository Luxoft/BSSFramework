using System.CodeDom;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Projection;
using Framework.Security;

namespace Framework.DomainDriven.ProjectionGenerator;

public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    private readonly IProjectionEnvironment projectionEnvironment;

    protected GeneratorConfigurationBase(TEnvironment environment, IProjectionEnvironment projectionEnvironment = null)
            : base(environment)
    {
        this.projectionEnvironment = projectionEnvironment
                                     ?? this.Environment.ProjectionEnvironments.SingleOrDefault()
                                     ?? throw new InvalidOperationException("ProjectionEnvironment not initialized");
    }

    public override string Namespace => this.projectionEnvironment.Namespace;

    protected override string NamespacePostfix { get; } = "Domain.Projection";

    /// <inheritdoc />
    public virtual bool OneToOneSetter { get; } = true;

    /// <inheritdoc />
    public virtual bool GeneratePublicCtors { get; } = false;

    protected virtual ICodeFileFactoryHeader<FileType> ProjectionFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.Projection, string.Empty, domainType => domainType.Name);

    protected virtual ICodeFileFactoryHeader<FileType> CustomProjectionBaseFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.CustomProjectionBase, string.Empty, domainType => $"Custom{domainType.Name}Base");


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
            foreach (var projectionFilterAttribute in domainType.GetCustomAttributes<ProjectionFilterAttribute>())
            {
                yield return projectionFilterAttribute.ToAttributeDeclaration();
            }
        }

        {
            foreach (var securityAttribute in this.Environment.ExtendedMetadata.GetType(domainType).GetSecurityAttributes(this.Environment.SecurityOperationType))
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

        foreach (var securityAttribute in this.Environment.ExtendedMetadata.GetProperty(property).GetSecurityAttributes(this.Environment.SecurityOperationType))
        {
            yield return securityAttribute;
        }

        if (property.GetCustomAttribute<CustomSerializationAttribute>(attr => attr.Role.HasFlag(DTORole.Client)) is var customSerializationAttr && customSerializationAttr != null)
        {
            yield return customSerializationAttr.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<ProjectionPropertyAttribute>() is var projectionPropAttr && projectionPropAttr != null)
        {
            yield return projectionPropAttr.ToAttributeDeclaration();
        }

        if (property.HasAttribute<ExpandPathAttribute>())
        {
            yield return property.GetCustomAttribute<ExpandPathAttribute>().ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<MappingAttribute>() is var mappingAttr && mappingAttr != null)
        {
            yield return mappingAttr.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<IgnoreFetchAttribute>() is var ignoreFetchAttribute && ignoreFetchAttribute != null)
        {
            yield return ignoreFetchAttribute.ToAttributeDeclaration();
        }

        foreach (var fetchPathAttribute in property.GetCustomAttributes<FetchPathAttribute>())
        {
            yield return fetchPathAttribute.ToAttributeDeclaration();
        }

        if (property.GetCustomAttribute<MappingPropertyAttribute>() is var mappingPropertyAttribute && mappingPropertyAttribute != null)
        {
            yield return mappingPropertyAttribute.ToAttributeDeclaration();
        }
    }

    protected override IEnumerable<Type> GetDomainTypes()
    {
        return this.projectionEnvironment.Assembly.GetTypes();
    }

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        yield return this.ProjectionFileFactoryHeader;
        yield return this.CustomProjectionBaseFileFactoryHeader;
    }
}
