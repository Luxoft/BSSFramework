using CommonFramework;

namespace Framework.Configuration.BLL;

public class DomainObjectVersionsResolverFactory(IConfigurationBLLContext bllContext, IServiceProxyFactory serviceProxyFactory) : IDomainObjectVersionsResolverFactory
{
    public IDomainObjectVersionsResolver Create(Type domainObjectType)
    {
        var targetSystemService = bllContext.GetTargetSystemService(domainObjectType, true);

        var domainObjectVersionsResolverType = typeof(DomainObjectVersionsResolver<,>).MakeGenericType(targetSystemService.BLLContextType, domainObjectType);

        return serviceProxyFactory.Create<IDomainObjectVersionsResolver>(domainObjectVersionsResolverType);
    }
}
