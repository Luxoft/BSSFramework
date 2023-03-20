using System;

using Framework.HierarchicalExpand;
using Framework.Projection;

namespace Framework.DomainDriven;

public class ProjectionHierarchicalRealTypeResolver : IHierarchicalRealTypeResolver
{
    public Type Resolve(Type identity) => identity.GetProjectionSourceTypeOrSelf();
}
