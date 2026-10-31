using Framework.BLL.Domain.Fetching;

namespace Framework.Projection.Lambda.ProjectionSource.AttributeSource;

/// <summary>
/// Attribute source for a custom projection property
/// </summary>
public class ProjectionCustomPropertyAttributeSource : AttributeSourceBase<IProjectionCustomProperty>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="environment">Environment</param>
    /// <param name="customProjectionProperty">Custom projection property</param>
    public ProjectionCustomPropertyAttributeSource(ProjectionLambdaEnvironment environment, IProjectionCustomProperty customProjectionProperty)
        : base(environment, customProjectionProperty)
    {
    }


    protected override IEnumerable<Attribute> GetDefaultAttributes() =>
        new Attribute?[]
        {
            this.TryCreateIgnoreFetchAttribute(),
            this.CreateProjectionPropertyAttribute()
        }.Where(attr => attr is not null).Select(v => v!).Concat(this.GetFetchPathAttributes());

    protected virtual IgnoreFetchAttribute? TryCreateIgnoreFetchAttribute()
    {
        if (this.ProjectionValue.Fetchs.Any())
        {
            return null;
        }
        else
        {
            return new IgnoreFetchAttribute();
        }
    }

    protected virtual ProjectionPropertyAttribute CreateProjectionPropertyAttribute() => new(ProjectionPropertyRole.Custom);

    private IEnumerable<FetchPathAttribute> GetFetchPathAttributes()
    {
        foreach (var path in this.ProjectionValue.Fetchs)
        {
            yield return new FetchPathAttribute(path);
        }
    }
}
