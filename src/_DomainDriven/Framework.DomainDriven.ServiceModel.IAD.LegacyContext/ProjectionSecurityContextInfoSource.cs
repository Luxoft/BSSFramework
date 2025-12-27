using CommonFramework.DependencyInjection;

using HierarchicalExpand;

using SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoSource(
    IServiceProxyFactory serviceProxyFactory,
    IEnumerable<SecurityContextInfo> securityContextInfoList,
    IRealTypeResolver realTypeResolver)
    : SecurityContextInfoSource(serviceProxyFactory, securityContextInfoList)
{
    public override SecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(realTypeResolver.Resolve(type));
    }
}
