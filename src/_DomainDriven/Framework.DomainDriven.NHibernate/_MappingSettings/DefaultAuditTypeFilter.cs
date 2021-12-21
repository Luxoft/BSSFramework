using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.NHibernate
{
    public class DefaultAuditTypeFilter : IAuditTypeFilter
    {
        private IList<IAuditTypeFilter> _filters;

        public DefaultAuditTypeFilter()
        {
            this._filters = new IAuditTypeFilter[]
                           {
                               new AuditTypeFilterSerivce<ViewAttribute>(),
                               new AuditTypeFilterSerivce<NotAuditedClassAttribute>(),
                               new AuditPropertyFilterService<NotAuditedPropertyAttribute>(),
                               new AuditPropertyFilterService<DetailRoleAttribute>(z=>z.Role == DetailRole.No),
                           };
        }

        public bool IsAuditedType(Type type)
        {
            return this._filters.All(z => z.IsAuditedType(type));
        }

        public bool IsAuditedProperty(Type type, PropertyInfo propertyInfo)
        {
            return this._filters.All(z => z.IsAuditedProperty(type, propertyInfo));
        }

        private class AuditTypeFilterSerivce<TAttribute> : IAuditTypeFilter
            where TAttribute : Attribute
        {
            public bool IsAuditedType(Type type)
            {
                return !type.HasAttribute<TAttribute>();
            }

            public bool IsAuditedProperty(Type type, PropertyInfo propertyInfo)
            {
                var propertyType = propertyInfo.PropertyType;

                return !(propertyType.HasAttribute<TAttribute>() || propertyType.GetCollectionOrArrayElementType().Maybe(z => z.HasAttribute<TAttribute>(), false));
            }
        }

        private class AuditPropertyFilterService<TAttribute> : IAuditTypeFilter
            where TAttribute : Attribute
        {
            private readonly Func<TAttribute, bool> skipAuditFunc;

            public AuditPropertyFilterService() : this(z => true)
            {

            }
            public AuditPropertyFilterService(Func<TAttribute, bool> skipAuditFunc)
            {
                this.skipAuditFunc = skipAuditFunc;
            }
            public bool IsAuditedType(Type type)
            {
                return true;
            }

            public bool IsAuditedProperty(Type type, PropertyInfo propertyInfo)
            {
                var attributes = propertyInfo.GetCustomAttributes<TAttribute>();
                if (!attributes.Any())
                {
                    return true;
                }
                var attribute = attributes.Single(() =>new System.ArgumentException(
                                                           $"Attribute:{typeof(Attribute).Name} from property:{propertyInfo.Name} more then one"));

                var skip = this.skipAuditFunc(attribute);

                return !skip;
            }
        }
    }
}