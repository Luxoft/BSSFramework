namespace Framework.Projection.Lambda.ProjectionSource.AutoProjection;

internal class LastProjectionProperty
{
    public LastProjectionProperty(string propertyName, ProjectionBuilder.ProjectionBuilder elementProjection)
    {
        this.PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        this.ElementProjection = elementProjection;
    }

    public string PropertyName { get; }

    public ProjectionBuilder.ProjectionBuilder ElementProjection { get; }
}
