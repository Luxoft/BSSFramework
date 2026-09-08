using System.Collections.Concurrent;

using Framework.Core;
using Framework.Database.InlineAudit;

namespace Framework.Database.NHibernate.InlineAudit;

public class AuditPropertiesSetterMapFactory(IEnumerable<InlineAuditBinding> bindings) : IAuditPropertiesSetterMapFactory
{
    private readonly ConcurrentDictionary<(Type, InlineAuditAction), IAuditPropertiesSetterMap> cache = [];

    public IAuditPropertiesSetterMap CreateSetterMap(Type domainObjectType, InlineAuditAction inlineAuditAction, IReadOnlyList<string> propertyNames) =>
        this.cache.GetOrAdd(
            (domainObjectType, inlineAuditAction),
            _ =>
            {
                var maps = from binding in bindings

                           where binding.Action == inlineAuditAction && binding.DomainObjectType.IsAssignableFrom(domainObjectType)

                           let propertyIndex = propertyNames.IndexOf(binding.Property.Name, StringComparer.InvariantCultureIgnoreCase)

                           where propertyIndex != -1

                           select new AuditPropertiesSetterMap(binding, propertyIndex);

                return maps.Aggregate();
            });
}
