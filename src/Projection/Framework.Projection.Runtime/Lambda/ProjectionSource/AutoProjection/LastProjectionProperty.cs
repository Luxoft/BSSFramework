namespace Framework.Projection.Lambda.ProjectionSource.AutoProjection;

internal class LastProjectionProperty(string propertyName, ProjectionBuilder.ProjectionBuilder elementProjection)
{
    public string PropertyName { get; } = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

    public ProjectionBuilder.ProjectionBuilder ElementProjection { get; } = elementProjection;
}
