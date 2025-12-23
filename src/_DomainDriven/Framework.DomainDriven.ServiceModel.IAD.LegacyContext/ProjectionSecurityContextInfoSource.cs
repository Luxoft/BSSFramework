using SecuritySystem;

using HierarchicalExpand;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoSource(
    IServiceProvider serviceProvider,
    IEnumerable<SecurityContextInfo> securityContextInfoList,
    IRealTypeResolver realTypeResolver)
    : SecurityContextInfoSource(serviceProvider, securityContextInfoList)
{
    public override SecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(realTypeResolver.Resolve(type));
    }
}
