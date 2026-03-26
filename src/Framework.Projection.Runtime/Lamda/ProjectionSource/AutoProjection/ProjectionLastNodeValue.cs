using System.Reflection;

namespace Framework.Projection.Lambda;

internal class ProjectionLastNodeValue : ProjectionNodeValue
{
    public ProjectionLastNodeValue(PropertyInfo property, LastProjectionProperty lastProperty)
            : base(property)
    {
        this.LastProperty = lastProperty ?? throw new ArgumentNullException(nameof(lastProperty));
    }

    public LastProjectionProperty LastProperty { get; }
}
