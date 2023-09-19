using Framework.HierarchicalExpand;
using Framework.Projection;

namespace Framework.DomainDriven;

public class ProjectionRealTypeResolver : IRealTypeResolver
{
    public Type Resolve(Type identity) => identity.GetProjectionSourceTypeOrSelf();
}
