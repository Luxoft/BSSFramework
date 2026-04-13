using System.Reflection;

namespace Framework.Projection.Lambda.ProjectionSource.AutoProjection;

internal record ProjectionLastNodeValue(PropertyInfo Property, LastProjectionProperty LastProperty) : ProjectionNodeValue(Property);
