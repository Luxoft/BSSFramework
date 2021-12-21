using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Framework.DomainDriven.SerializeMetadata
{
    [DataContract]
    public sealed class DomainTypeSubsetMetadata : DomainTypeMetadata<PropertySubsetMetadata, TypeMetadata>, IEquatable<DomainTypeSubsetMetadata>
    {
        public DomainTypeSubsetMetadata(TypeHeader type, TypeRole role, bool isHierarchical, IEnumerable<PropertySubsetMetadata> properties)
            : base(type, role, isHierarchical, properties)
        {
        }

        public bool Equals(DomainTypeSubsetMetadata other) => base.Equals(other);

        public DomainTypeSubsetMetadata OverrideHeader(Func<TypeHeader, TypeHeader> selector)
        {
            return (DomainTypeSubsetMetadata)this.OverrideHeaderBase(selector);
        }

        public override TypeMetadata OverrideHeaderBase(Func<TypeHeader, TypeHeader> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new DomainTypeSubsetMetadata(
                selector(this.Type),
                this.Role,
                this.IsHierarchical,
                this.Properties.Select(prop => prop.OverrideHeader(selector)));
        }
    }
}
