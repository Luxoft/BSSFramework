using System.Runtime.Serialization;

namespace Framework.DomainDriven.SerializeMetadata;

[DataContract]
public sealed class DomainTypeMetadata : DomainTypeMetadata<PropertyMetadata, TypeHeader>, IEquatable<DomainTypeMetadata>
{
    public DomainTypeMetadata(TypeHeader type, TypeRole role, bool isHierarchical, IEnumerable<PropertyMetadata> properties)
            : base(type, role, isHierarchical, properties)
    {
    }

    public bool Equals(DomainTypeMetadata other)
    {
        return base.Equals(other);
    }

    public override TypeMetadata OverrideHeaderBase(Func<TypeHeader, TypeHeader> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new DomainTypeMetadata(
                                      selector(this.Type),
                                      this.Role,
                                      this.IsHierarchical,
                                      this.Properties);
    }
}
