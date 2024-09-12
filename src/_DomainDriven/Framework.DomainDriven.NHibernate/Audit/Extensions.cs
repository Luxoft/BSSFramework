using System.Reflection;

using Framework.Core;

using NHibernate.Mapping;

namespace Framework.DomainDriven.NHibernate.Audit;

public static class Extensions
{
    public static IAuditAttributeService GetAuditAttributeService(this IList<MappingSettings> mappingSettings, IEnumerable<PersistentClass> persistentClasses)
    {
        var monitoriableTypes = mappingSettings
                                .Select(z => new { MappingSettings = z, Filter = z.AuditTypeFilter })
                                .SelectMany(z => z.MappingSettings.Types.Select(q => new { Filter = z.Filter, Type = q }))
                                .ToList();

        var typeToFilter = monitoriableTypes.Where(z => z.Filter.IsAuditedType(z.Type)).ToDictionary(z => z.Type, z => z.Filter);

        var types = typeToFilter.Keys.ToList();

        var filteredClassMappings = types.Join(
                                               persistentClasses,
                                               z => z.FullName,
                                               z => z.EntityName,
                                               (type, persistentClass) => new { Type = type, PersistentClass = persistentClass })
                                         .ToList();

        var auditService = new AuditEntityService();

        mappingSettings.Foreach(z =>
                                {
                                    auditService.Register((Type)z.PersistentDomainObjectBaseType, true);

                                    var order = Enumerable.OrderBy<Type, string>(z.Types, q => q.Name).ToList();

                                    z.Types.Foreach(q => auditService.Register((Type)q, (string)z.AuditDatabase.Schema));
                                });

        foreach (var pair in filteredClassMappings)
        {
            var persistentClass = pair.PersistentClass;

            var type = pair.Type;

            var filter = typeToFilter[pair.Type];

            auditService.Register(type, filter.IsAuditedType(type));

            var typeProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var join = typeProperties.Join(persistentClass.PropertyIterator, z => z.Name, z => z.Name, (propertyInfo, hibProperty) => new { propertyInfo, hibProperty }).ToList();

            join.Foreach(z => auditService.Register(z.hibProperty, filter.IsAuditedProperty(type, z.propertyInfo)));
        }

        return auditService;
    }
}
