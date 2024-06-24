using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven;

public struct FilterItemIdentity : IEquatable<FilterItemIdentity>
{

    public FilterItemIdentity(string securityContextTypeName, string securityContextId)
            : this(securityContextTypeName, new Guid(securityContextId))
    {

    }

    public FilterItemIdentity(string securityContextTypeName, Guid securityContextId)
            : this()
    {
        this.SecurityContextTypeName = securityContextTypeName;
        this.SecurityContextId = securityContextId;
    }


    public string SecurityContextTypeName { get; set; }

    public Guid SecurityContextId { get; set; }


    public override string ToString()
    {
        return $"SecurityContextId: {this.SecurityContextId}, SecurityContextTypeName: {this.SecurityContextTypeName}";
    }

    public override int GetHashCode()
    {
        return this.SecurityContextId.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return obj is FilterItemIdentity && this.Equals((FilterItemIdentity)obj);
    }

    public bool Equals(FilterItemIdentity other)
    {
        return this.SecurityContextId == other.SecurityContextId && this.SecurityContextTypeName == other.SecurityContextTypeName;
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
