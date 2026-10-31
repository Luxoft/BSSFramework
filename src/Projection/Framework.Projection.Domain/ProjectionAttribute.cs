namespace Framework.Projection;

/// <summary>
/// Attribute indicating that this type is a projection
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ProjectionAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sourceType">Source type the projection was built from</param>
    /// <param name="role">Projection role</param>
    /// <param name="contractType">Contract the projection was built from</param>
    public ProjectionAttribute(Type sourceType, ProjectionRole role, Type? contractType = null)
    {
        if (!Enum.IsDefined(typeof(ProjectionRole), role)) throw new ArgumentOutOfRangeException(nameof(role));

        this.SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
        this.Role = role;
        this.ContractType = contractType;
    }

    /// <summary>
    /// Source type the projection was built from
    /// </summary>
    public Type SourceType { get; }

    /// <summary>
    /// Projection role
    /// </summary>
    public ProjectionRole Role { get; }

    /// <summary>
    /// Contract the projection was built from
    /// </summary>
    public Type? ContractType { get; }
}
