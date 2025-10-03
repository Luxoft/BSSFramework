using Framework.Projection;

using SecuritySystem.HierarchicalExpand;

namespace Framework.DomainDriven;

public class ProjectionRealTypeResolver : IRealTypeResolver
{
    public Type Resolve(Type identity) => identity.GetProjectionSourceTypeOrSelf();
}
