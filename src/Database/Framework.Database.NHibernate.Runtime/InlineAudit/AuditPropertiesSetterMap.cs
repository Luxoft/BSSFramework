using Anch.DependencyInjection;

using Framework.Database.InlineAudit;

namespace Framework.Database.NHibernate.InlineAudit;

public class AuditPropertiesSetterMap(InlineAuditBinding binding, int propertyIndex) : IAuditPropertiesSetterMap
{
    public IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider)
    {
        var serviceProxyFactory = serviceProvider.GetServiceProxyFactory();

        var resolver = serviceProxyFactory.Create<IAuditValueResolver<object>>(binding.AuditValueResolverType);

        return new AuditPropertiesSetter(resolver, propertyIndex);
    }
}
