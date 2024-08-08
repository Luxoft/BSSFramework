using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoService(
    IEnumerable<ISecurityContextInfo<Guid>> securityContextInfoList,
    IRealTypeResolver realTypeResolver)
    : SecurityContextInfoService<Guid>(securityContextInfoList)
{
    public override ISecurityContextInfo<Guid> GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(realTypeResolver.Resolve(type));
    }
}
