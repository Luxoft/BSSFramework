using Anch.Core;

using Anch.HierarchicalExpand;

using Anch.SecuritySystem;

namespace Framework.Infrastructure;

public class ProjectionSecurityContextInfoSource(
    IServiceProxyFactory serviceProxyFactory,
    IEnumerable<SecurityContextInfo> securityContextInfoList,
    IActualDomainTypeResolver actualDomainTypeResolver)
    : SecurityContextInfoSource(serviceProxyFactory, securityContextInfoList)
{
    public override SecurityContextInfo GetSecurityContextInfo(Type type) => base.GetSecurityContextInfo(actualDomainTypeResolver.Resolve(type));
}
