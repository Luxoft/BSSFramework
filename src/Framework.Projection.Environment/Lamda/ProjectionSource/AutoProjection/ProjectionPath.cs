using System;

using Framework.Core;

namespace Framework.Projection.Lambda;

internal class ProjectionPath
{
    public ProjectionPath(PropertyPath propertyPath, LastProjectionProperty lastProperty)
    {
        this.PropertyPath = propertyPath ?? throw new ArgumentNullException(nameof(propertyPath));
        this.LastProperty = lastProperty ?? throw new ArgumentNullException(nameof(lastProperty));
    }

    public PropertyPath PropertyPath { get; }

    public LastProjectionProperty LastProperty { get; }
}
