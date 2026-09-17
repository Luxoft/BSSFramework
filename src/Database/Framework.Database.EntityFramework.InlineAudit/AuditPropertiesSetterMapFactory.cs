using System.Collections.Concurrent;

using Anch.Core;

using Framework.Database.InlineAudit;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditPropertiesSetterMapFactory(IServiceProxyFactory serviceProxyFactory, IEnumerable<InlineAuditBinding> bindings)
    : IAuditPropertiesSetterMapFactory
{
    private readonly ConcurrentDictionary<(Type, InlineAuditAction), IAuditPropertiesSetterMap> cache = [];

    public IAuditPropertiesSetterMap CreateSetterMap(Type domainObjectType, InlineAuditAction inlineAuditAction) =>
        this.cache.GetOrAdd(
            (domainObjectType, inlineAuditAction),
            _ =>
            {
                var maps = from binding in bindings

                           where binding.Action == inlineAuditAction && binding.DomainObjectType.IsAssignableFrom(domainObjectType)

                           select serviceProxyFactory.Create<IAuditPropertiesSetterMap>(
                               typeof(AuditPropertiesSetterMap<>).MakeGenericType(binding.Property.PropertyType),
                               binding);

                return maps.Aggregate();
            });
}
