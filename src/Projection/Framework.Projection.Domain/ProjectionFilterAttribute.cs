namespace Framework.Projection;

/// <summary>
/// Attribute indicating that this type is a projection filter
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ProjectionFilterAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="filterType">Filter type</param>
    /// <param name="target">Filter applicability</param>
    public ProjectionFilterAttribute(Type filterType, ProjectionFilterTargets target)
    {
        this.FilterType = filterType ?? throw new ArgumentNullException(nameof(filterType));
        this.Target = target;
    }

    /// <summary>
    /// Filter type
    /// </summary>
    public Type FilterType { get; }

    /// <summary>
    /// Filter applicability
    /// </summary>
    public ProjectionFilterTargets Target { get; }
}
