using System.Reflection;

namespace Framework.Projection.Lambda.ProjectionSource.AutoProjection;

internal class ProjectionLastNodeValue(PropertyInfo property, LastProjectionProperty lastProperty) : ProjectionNodeValue(property)
{
    public LastProjectionProperty LastProperty { get; } = lastProperty ?? throw new ArgumentNullException(nameof(lastProperty));
}
