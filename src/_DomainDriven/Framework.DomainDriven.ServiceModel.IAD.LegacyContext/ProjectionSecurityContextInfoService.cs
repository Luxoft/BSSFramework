using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoService(
    IEnumerable<ISecurityContextInfo> securityContextInfoList,
    IRealTypeResolver realTypeResolver)
    : SecurityContextInfoService(securityContextInfoList)
{
    public override ISecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(realTypeResolver.Resolve(type));
    }
}
