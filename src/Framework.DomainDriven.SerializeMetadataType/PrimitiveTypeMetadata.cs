using System;
using System.Runtime.Serialization;

namespace Framework.DomainDriven.SerializeMetadata
{
    [DataContract]
    public class PrimitiveTypeMetadata : TypeMetadata
    {
        public PrimitiveTypeMetadata(TypeHeader type)
            : base(type, TypeRole.Primitive)
        {
        }

        public override TypeMetadata OverrideHeaderBase(Func<TypeHeader, TypeHeader> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new PrimitiveTypeMetadata(selector(this.Type));
        }
    }
}
