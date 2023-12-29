using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionSecurityContextInfoService : SecurityContextInfoService
{
    private readonly IRealTypeResolver realTypeResolver;

    public ProjectionSecurityContextInfoService(
        IEnumerable<ISecurityContextInfo> securityContextInfoList,
        IRealTypeResolver realTypeResolver)
        : base(securityContextInfoList)
    {
        this.realTypeResolver = realTypeResolver;
    }

    public override ISecurityContextInfo GetSecurityContextInfo(Type type)
    {
        return base.GetSecurityContextInfo(this.realTypeResolver.Resolve(type));
    }
}
