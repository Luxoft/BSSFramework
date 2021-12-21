using System;
using System.Runtime.Serialization;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.SerializeMetadata
{
    public interface ITypeMetadata : IIdentityObject<TypeHeader>
    {
        TypeHeader Type { get; }

        TypeRole Role { get; }
    }

    [DataContract]
    [KnownType(typeof(EnumMetadata)), KnownType(typeof(DomainTypeMetadata)), KnownType(typeof(PrimitiveTypeMetadata))]
    public abstract class TypeMetadata : ITypeMetadata, ITypeMap, IEquatable<TypeMetadata>
    {
        protected TypeMetadata(TypeHeader type, TypeRole role)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Role = role;
        }

        [DataMember]
        public TypeHeader Type { get; private set; }

        [DataMember]
        public TypeRole Role { get; private set; }


        public override string ToString()
        {
            return this.Type.Name;
        }

        #region IIdentityObject<TypeHeader> Members

        TypeHeader IIdentityObject<TypeHeader>.Id
        {
            get { return this.Type; }
        }

        #endregion

        #region ITypeMap Members

        string ITypeMap.Name
        {
            get { return this.Type.Name; }
        }

        #endregion

        #region IEquatable<TypeMetadata> Members

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() ^ this.Role.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TypeMetadata);
        }

        public bool Equals(TypeMetadata other)
        {
            return other != null && this.Type == other.Type && this.Role == other.Role;
        }

        #endregion

        public abstract TypeMetadata OverrideHeaderBase(Func<TypeHeader, TypeHeader> selector);
    }
}
