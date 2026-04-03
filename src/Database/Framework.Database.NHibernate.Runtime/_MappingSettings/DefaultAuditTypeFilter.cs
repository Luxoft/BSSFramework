using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Database.Mapping;
using Framework.Relations;

namespace Framework.Database.NHibernate._MappingSettings;

public class DefaultAuditTypeFilter : IAuditTypeFilter
{
    private readonly IReadOnlyList<IAuditTypeFilter> filters =
    [
        new AuditTypeFilterService<ViewAttribute>(),
        new AuditTypeFilterService<NotAuditedClassAttribute>(),
        new AuditPropertyFilterService<NotAuditedPropertyAttribute>(),
        new AuditPropertyFilterService<DetailRoleAttribute>(z => z.Role == DetailRole.No)
    ];

    public bool IsAuditedType(Type type) => this.filters.All(z => z.IsAuditedType(type));

    public bool IsAuditedProperty(Type type, PropertyInfo propertyInfo) => this.filters.All(z => z.IsAuditedProperty(type, propertyInfo));

    private class AuditTypeFilterService<TAttribute> : IAuditTypeFilter
        where TAttribute : Attribute
    {
        public bool IsAuditedType(Type type) => !type.HasAttribute<TAttribute>();

        public bool IsAuditedProperty(Type type, PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;

            return !(propertyType.HasAttribute<TAttribute>() || propertyType.GetCollectionOrArrayElementType().Maybe(z => z.HasAttribute<TAttribute>(), false));
        }
    }

    private class AuditPropertyFilterService<TAttribute>(Func<TAttribute, bool> skipAuditFunc) : IAuditTypeFilter
        where TAttribute : Attribute
    {
        public AuditPropertyFilterService()
            : this(z => true)
        {

        }

        public bool IsAuditedType(Type type) => true;

        public bool IsAuditedProperty(Type type, PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes<TAttribute>();
            if (!attributes.Any())
            {
                return true;
            }

            var attribute = attributes.Single(() => new ArgumentException(
                                                  $"Attribute:{nameof(Attribute)} from property:{propertyInfo.Name} more then one"));

            var skip = skipAuditFunc(attribute);

            return !skip;
        }
    }
}
