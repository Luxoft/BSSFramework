using Framework.Core;


namespace Framework.Projection.Lambda.ProjectionSource.AutoProjection;

internal class ProjectionPath(PropertyPath propertyPath, LastProjectionProperty lastProperty)
{
    public PropertyPath PropertyPath { get; } = propertyPath ?? throw new ArgumentNullException(nameof(propertyPath));

    public LastProjectionProperty LastProperty { get; } = lastProperty ?? throw new ArgumentNullException(nameof(lastProperty));
}
