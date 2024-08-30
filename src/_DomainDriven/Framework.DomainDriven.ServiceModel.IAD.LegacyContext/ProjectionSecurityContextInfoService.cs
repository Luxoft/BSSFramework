using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextSource(
    IEnumerable<SecurityContextInfo> securityContextInfoList,
    IRealTypeResolver realTypeResolver)
    : SecurityContextSource(securityContextInfoList)
{
    public override SecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(realTypeResolver.Resolve(type));
    }
}
