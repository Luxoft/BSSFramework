using System.Reflection;

namespace Framework.Projection.Lambda.ProjectionSource.AutoProjection;

internal class ProjectionNodeValue(PropertyInfo property)
{
    public PropertyInfo Property { get; } = property ?? throw new ArgumentNullException(nameof(property));
}
