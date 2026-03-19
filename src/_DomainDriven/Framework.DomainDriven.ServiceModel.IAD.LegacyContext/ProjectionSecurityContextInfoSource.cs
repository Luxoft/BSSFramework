using CommonFramework;

using HierarchicalExpand;

using SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoSource(
    IServiceProxyFactory serviceProxyFactory,
    IEnumerable<SecurityContextInfo> securityContextInfoList,
    IActualDomainTypeResolver actualDomainTypeResolver)
    : SecurityContextInfoSource(serviceProxyFactory, securityContextInfoList)
{
    public override SecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(actualDomainTypeResolver.Resolve(type));
    }
}
