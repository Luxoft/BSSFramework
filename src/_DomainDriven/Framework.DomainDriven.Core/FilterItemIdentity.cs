using System;

using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security
{
    public struct FilterItemIdentity : IDefaultIdentityObject, IEquatable<FilterItemIdentity>
    {
        public FilterItemIdentity(FilterItemType entityType, string id)
            : this(entityType.ToString(), id)
        {

        }

        public FilterItemIdentity(string entityName, string id)
            : this(entityName, new Guid(id))
        {

        }

        public FilterItemIdentity(FilterItemType entityType, Guid id)
            : this(entityType.ToString(), id)
        {

        }

        public FilterItemIdentity(string entityName, Guid id)
            : this()
        {
            this.EntityName = entityName;
            this.Id = id;
        }


        public string EntityName { get; set; }

        public Guid Id { get; set; }


        public override string ToString()
        {
            return $"Id: {this.Id}, EntityType: {this.EntityName}";
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is FilterItemIdentity && this.Equals((FilterItemIdentity)obj);
        }

        public bool Equals(FilterItemIdentity other)
        {
            return this.Id == other.Id && this.EntityName == other.EntityName;
        }
    }


    public static class FilterItemIdentityExtensions
    {
        public static FilterItemIdentity ToFilterItemIdentity(this string typeName, Guid id)
        {
            if (typeName == null) throw new ArgumentNullException(nameof(typeName));

            return new FilterItemIdentity(typeName, id);
        }

        public static FilterItemIdentity ToFilterItemIdentity(this Type type, Guid id)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.Name.ToFilterItemIdentity(id);
        }

        public static FilterItemIdentity ToFilterItemIdentity<TDomainObject>(this TDomainObject domainObject)
            where TDomainObject : class, ISecurityContext, IDefaultIdentityObject
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return new FilterItemIdentity(typeof(TDomainObject).Name, domainObject.Id);
        }
    }

    public enum FilterItemType
    {
        Location,

        BusinessUnit,

        ManagementUnit,

        Employee
    }
}
