using System.Reflection;

using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.ServiceRole;
using Framework.BLL.Domain.ServiceRole.Base;
using Framework.Database.Mapping;
using Framework.Validation;
using Framework.Validation.Attributes._Base;

namespace Framework.Projection.Lambda.ProjectionSource.AttributeSource;

/// <summary>
/// Источник атрибутов для типа проекции
/// </summary>
public class ProjectionTypeAttributeSource : AttributeSourceBase<IProjection>
{
    private readonly bool isPersistent;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="environment">Окружение</param>
    /// <param name="projection">Тип на основе которого строится проекция</param>
    public ProjectionTypeAttributeSource(ProjectionLambdaEnvironment environment, IProjection projection)
        : base(environment, projection) =>
        this.isPersistent = environment.PersistentDomainObjectBaseType.IsAssignableFrom(this.SourceType);

    /// <summary>
    /// Тип на основе которого строится проекция
    /// </summary>
    public Type SourceType => this.ProjectionValue.SourceType;

    private Type WrappedSourceType => field ??= this.Environment.MetadataProxyProvider.Wrap(this.SourceType);

    /// <summary>
    /// Флаг указания, что проекция наследуется от базовой секурной проекции.
    /// </summary>
    protected bool HasBaseSecurityType => this.ProjectionValue.Role == ProjectionRole.Default
                                          && !this.Environment.UseDependencySecurity
                                          && this.WrappedSourceType.HasSecurityNodeInterfaces();


    /// <inheritdoc />
    protected override IEnumerable<Attribute> GetDefaultAttributes()
    {
        var attributes = new Attribute[]
                         {
                             this.TryCreateBLLProjectionViewRole(),
                             this.CreateTableAttribute(),
                             this.CreateInlineBaseTypeAttribute(),
                             this.CreateProjectionAttribute()
                         }.Where(attr => attr != null);

        return attributes.Concat(this.ProjectionValue.FilterAttributes)
                         .Concat(this.GetSourceTypeAttributes())
                         .Concat(this.GetDomainObjectAccessAttributes());
    }

    private IEnumerable<Attribute> GetSourceTypeAttributes() =>
        this.WrappedSourceType.GetCustomAttributes().Where(attr =>
                                                        !(attr is TableAttribute)
                                                        && !(attr is BLLRoleAttribute)
                                                        && !(attr is ClassValidatorAttribute)
                                                        && !(attr is DomainObjectAccessAttribute)
                                                        && !(attr is DependencySecurityAttribute));

    private IEnumerable<Attribute> GetDomainObjectAccessAttributes()
    {
        if (this.Environment.UseDependencySecurity)
        {
            if (this.isPersistent)
            {
                yield return new DependencySecurityAttribute(this.SourceType);
            }
        }
        else
        {
            foreach (var attr in this.WrappedSourceType.GetCustomAttributes().Where(attr => attr is DomainObjectAccessAttribute))
            {
                yield return attr;
            }
        }
    }

    private Attribute? CreateInlineBaseTypeAttribute()
    {
        if (this.HasBaseSecurityType)
        {
            return new InlineBaseTypeMappingAttribute();
        }

        return null;
    }

    private BLLProjectionViewRoleAttribute? TryCreateBLLProjectionViewRole()
    {
        if (this.ProjectionValue.BLLView)
        {
            return new BLLProjectionViewRoleAttribute();
        }
        else
        {
            return null;
        }
    }

    private TableAttribute CreateTableAttribute()
    {
        var result = this.WrappedSourceType.GetCustomAttribute<TableAttribute>() ?? new TableAttribute { Name = this.SourceType.Name };

        return result;
    }

    private ProjectionAttribute CreateProjectionAttribute() => new(this.SourceType, this.ProjectionValue.Role);
}
