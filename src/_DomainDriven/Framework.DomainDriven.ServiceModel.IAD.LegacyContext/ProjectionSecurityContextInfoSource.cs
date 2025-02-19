using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoSource(
    IEnumerable<SecurityContextInfo> securityContextInfoList,
    IRealTypeResolver realTypeResolver)
    : SecurityContextInfoSource(securityContextInfoList)
{
    public override SecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(realTypeResolver.Resolve(type));
    }
}
