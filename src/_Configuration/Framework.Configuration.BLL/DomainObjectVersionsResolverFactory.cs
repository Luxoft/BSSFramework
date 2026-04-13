using CommonFramework;

using Framework.BLL.Services;

namespace Framework.Configuration.BLL;

public class DomainObjectVersionsResolverFactory(IServiceProxyFactory serviceProxyFactory, ITargetSystemInfoService targetSystemInfoService) : IDomainObjectVersionsResolverFactory
{
    public IDomainObjectVersionsResolver Create(Type domainObjectType)
    {
        var targetSystemInfo = targetSystemInfoService.GetPersistentTargetSystemInfo(domainObjectType);

        var domainObjectVersionsResolverType = typeof(DomainObjectVersionsResolver<,>).MakeGenericType(targetSystemInfo.BllContextType, domainObjectType);

        return serviceProxyFactory.Create<IDomainObjectVersionsResolver>(domainObjectVersionsResolverType);
    }
}
