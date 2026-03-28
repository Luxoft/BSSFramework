using Framework.Projection;

using HierarchicalExpand;

namespace Framework.Infrastructure;

public class ProjectionActualDomainTypeResolver : IActualDomainTypeResolver
{
    public Type Resolve(Type identity) => identity.GetProjectionSourceTypeOrSelf();
}
