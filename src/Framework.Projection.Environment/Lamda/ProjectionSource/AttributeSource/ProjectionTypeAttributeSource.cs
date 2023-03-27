using System.Reflection;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Security;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

/// <summary>
/// Источник атрибутов для типа проекции
/// </summary>
public class ProjectionTypeAttributeSource : AttributeSourceBase<IProjection>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="environment">Окружение</param>
    /// <param name="projection">Тип на основе которого строится проекция</param>
    public ProjectionTypeAttributeSource([NotNull] ProjectionLambdaEnvironment environment, [NotNull] IProjection projection)
            : base(environment, projection)
    {
    }


    /// <summary>
    /// Тип на основе которого строится проекция
    /// </summary>
    public Type SourceType => this.ProjectionValue.SourceType;

    /// <summary>
    /// Флаг указания, что проекция наследуется от базовой секурной проекции.
    /// </summary>
    protected bool HasBaseSecurityType => this.ProjectionValue.Role == ProjectionRole.Default
                                          && !this.Environment.UseDependencySecurity
                                          && this.ProjectionValue.SourceType.HasSecurityNodeInterfaces();


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

    private IEnumerable<Attribute> GetSourceTypeAttributes()
    {
        return this.SourceType.GetCustomAttributes().Where(attr =>
                                                                   !(attr is TableAttribute)
                                                                   && !(attr is BLLRoleAttribute)
                                                                   && !(attr is ClassValidatorAttribute)
                                                                   && !(attr is DomainTypeAttribute)
                                                                   && !(attr is DomainObjectAccessAttribute)
                                                                   && !(attr is DependencySecurityAttribute)
                                                                   && !(attr is CustomContextSecurityAttribute));
    }

    private IEnumerable<Attribute> GetDomainObjectAccessAttributes()
    {
        if (this.Environment.UseDependencySecurity)
        {
            if (this.SourceType.IsSecurity())
            {
                yield return new DependencySecurityAttribute(this.SourceType);
            }
        }
        else
        {
            foreach (var attr in this.SourceType.GetCustomAttributes().Where(attr => attr is DomainObjectAccessAttribute))
            {
                yield return attr;
            }
        }
    }

    private Attribute CreateInlineBaseTypeAttribute()
    {
        if (this.HasBaseSecurityType)
        {
            return new InlineBaseTypeMappingAttribute();
        }

        return null;
    }

    private BLLProjectionViewRoleAttribute TryCreateBLLProjectionViewRole()
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
        var result = this.SourceType.GetCustomAttribute<TableAttribute>() ?? new TableAttribute { Name = this.SourceType.Name };

        return result;
    }

    private ProjectionAttribute CreateProjectionAttribute()
    {
        return new ProjectionAttribute(this.SourceType, this.ProjectionValue.Role);
    }
}
