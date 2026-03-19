using Framework.Projection;

using HierarchicalExpand;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionActualDomainTypeResolver : IActualDomainTypeResolver
{
    public Type Resolve(Type identity) => identity.GetProjectionSourceTypeOrSelf();
}
