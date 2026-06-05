using Anch.HierarchicalExpand;

using Framework.Projection;

namespace Framework.Infrastructure;

public class ProjectionActualDomainTypeResolver : IActualDomainTypeResolver
{
    public Type Resolve(Type identity) => identity.GetProjectionSourceTypeOrSelf();
}

