using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.Metadata
{
    public class ReferenceTypeFieldMetadata : FieldMetadata
    {
        public ReferenceTypeFieldMetadata (string name, Type type, IEnumerable<Attribute> attributes, DomainTypeMetadata domainTypeMetadata)
            : base (name, type, attributes, domainTypeMetadata)
        {

        }

        public Type ToType
        {
            get { return this.Type; }
        }

        public Type FromType
        {
            get { return this.DomainTypeMetadata.DomainType; }
        }

        public bool IsMasterReference
        {
            get { return this.Attributes.OfType<IsMasterAttribute>().Any(); }
        }

        public bool IsOneToOneReference
        {
            get { return this.Attributes.OfType<MappingAttribute>().Any(attribute => attribute.IsOneToOne); }
        }

        public CascadeMode CascadeMode
        {
            get
            {
                var attributes = this.Attributes.OfType<MappingAttribute>().Where(attribute => attribute.CascadeMode != CascadeMode.Auto).ToList();

                return attributes.Any() ? (attributes.First().GetActualCascadeMode() ?? CascadeMode.Disabled) : CascadeMode.Disabled;
            }
        }

    }
}