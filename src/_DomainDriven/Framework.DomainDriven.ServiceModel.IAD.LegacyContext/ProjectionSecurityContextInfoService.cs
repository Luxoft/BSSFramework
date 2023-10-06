using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoService : SecurityContextInfoService
{
    private readonly IRealTypeResolver realTypeResolver;

    public ProjectionSecurityContextInfoService(
        IEnumerable<SecurityContextInfo> securityContextInfoList,
        IRealTypeResolver realTypeResolver)
        : base(securityContextInfoList)
    {
        this.realTypeResolver = realTypeResolver;
    }

    public override SecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(this.realTypeResolver.Resolve(type));
    }
}
