using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.DomainDriven.SerializeMetadata;

[DataContract]
public abstract class DomainTypeMetadata<TPropertyMetadata, TPropertyType> : TypeMetadata, IDomainTypeMetadata, IEquatable<DomainTypeMetadata<TPropertyMetadata, TPropertyType>>
        where TPropertyMetadata : PropertyMetadata<TPropertyType>, IEquatable<TPropertyMetadata>
        where TPropertyType : IEquatable<TPropertyType>
{
    protected DomainTypeMetadata(TypeHeader type, TypeRole role, bool isHierarchical, IEnumerable<TPropertyMetadata> properties)
            : base(type, role)
    {
        this.IsHierarchical = isHierarchical;
        this.Properties = properties.CheckNotNull().ToReadOnlyCollection();
    }

    [DataMember]
    public bool IsHierarchical { get; private set; }

    [DataMember]
    public ReadOnlyCollection<TPropertyMetadata> Properties { get; private set; }

    IEnumerable<IPropertyMetadata> IDomainTypeMetadata.Properties => this.Properties;

    public bool Equals(DomainTypeMetadata<TPropertyMetadata, TPropertyType> other)
    {
        return base.Equals(other) && this.Properties.SequenceEqual(other.Properties);
    }
}
