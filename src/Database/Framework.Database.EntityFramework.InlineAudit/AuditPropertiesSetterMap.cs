using Anch.DependencyInjection;

using Framework.Database.InlineAudit;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditPropertiesSetterMap<TProperty>(InlineAuditBinding binding) : IAuditPropertiesSetterMap
{
    public IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider)
    {
        var serviceProxyFactory = serviceProvider.GetServiceProxyFactory();

        var resolver = serviceProxyFactory.Create<IAuditValueResolver<TProperty>>(binding.AuditValueResolverType);

        return new AuditPropertiesSetter<TProperty>(resolver, binding.Property.Name);
    }
}
