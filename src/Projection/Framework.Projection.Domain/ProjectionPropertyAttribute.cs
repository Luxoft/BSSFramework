namespace Framework.Projection;

/// <summary>
/// Attribute marking a projection property
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ProjectionPropertyAttribute : Attribute
{
    public ProjectionPropertyAttribute(ProjectionPropertyRole role)
    {
        if (!Enum.IsDefined(typeof(ProjectionPropertyRole), role)) throw new ArgumentOutOfRangeException(nameof(role));

        this.Role = role;
    }


    /// <summary>
    /// Projection property role
    /// </summary>
    public ProjectionPropertyRole Role { get; }
}
