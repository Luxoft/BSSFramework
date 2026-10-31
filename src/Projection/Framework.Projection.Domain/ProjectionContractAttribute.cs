namespace Framework.Projection;

/// <summary>
/// Attribute indicating that this interface is a projection
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public class ProjectionContractAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sourceType">Source type the projection will be built from</param>
    public ProjectionContractAttribute(Type sourceType)
    {
        if (sourceType is null) throw new ArgumentNullException(nameof(sourceType));

        if (sourceType.IsAbstract)
        {
            throw new Exception($"Type {sourceType.Name} can't be abstract");
        }

        if (sourceType.IsGenericTypeDefinition)
        {
            throw new Exception($"Type {sourceType.Name} can't be generic");
        }

        this.SourceType = sourceType;
    }

    /// <summary>
    /// Source type the projection will be built from
    /// </summary>
    public Type SourceType { get; }
}
