using Anch.DependencyInjection;

using Framework.Database.InlineAudit;

namespace Framework.Database.NHibernate.InlineAudit;

public class AuditPropertiesSetterMap<TProperty>(InlineAuditBinding binding, int propertyIndex) : IAuditPropertiesSetterMap
{
    public IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider)
    {
        var serviceProxyFactory = serviceProvider.GetServiceProxyFactory();

        var resolver = serviceProxyFactory.Create<IAuditValueResolver<TProperty>>(binding.AuditValueResolverType);

        return new AuditPropertiesSetter<TProperty>(resolver, propertyIndex);
    }
}
