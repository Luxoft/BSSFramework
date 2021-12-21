using System;
using System.Runtime.Serialization;

using Framework.Persistent;

namespace Framework.DomainDriven.SerializeMetadata
{
    [DataContract]
    public class PropertySubsetMetadata : PropertyMetadata<TypeMetadata>, IEquatable<PropertySubsetMetadata>
    {
        public PropertySubsetMetadata(string name, TypeMetadata type, bool isCollection, bool allowNull, bool isVirtual, bool isSecurity, bool isVisualIdentity, string alias)
            : base(name, type, isCollection, allowNull, isVirtual, isSecurity, isVisualIdentity)
        {
            this.Alias = alias;
        }


        public string Alias { get; private set; }


        public override TypeHeader TypeHeader
        {
            get { return this.Type.Type; }
        }


        public bool Equals(PropertySubsetMetadata other)
        {
            return base.Equals(other) && this.Alias == other.Alias;
        }

        public PropertySubsetMetadata OverrideHeader(Func<TypeHeader, TypeHeader> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new PropertySubsetMetadata(
                this.Name,
                this.Type.OverrideHeaderBase(selector),
                this.IsCollection,
                this.AllowNull,
                this.IsVirtual,
                this.IsSecurity,
                this.IsVisualIdentity,
                this.Alias);
        }
    }

    [DataContract]
    public class PropertyMetadata : PropertyMetadata<TypeHeader>, IEquatable<PropertyMetadata>
    {
        public PropertyMetadata(string name, TypeHeader type, bool isCollection, bool allowNull, bool isVirtual, bool isSecurity, bool isVisualIdentity)
            : base(name, type, isCollection, allowNull, isVirtual, isSecurity, isVisualIdentity)
        {

        }

        public override TypeHeader TypeHeader
        {
            get { return this.Type; }
        }


        public bool Equals(PropertyMetadata other)
        {
            return base.Equals(other);
        }
    }

    public interface IPropertyMetadata : IVisualIdentityObject
    {
        TypeHeader TypeHeader { get; }

        bool IsCollection { get; }

        bool AllowNull { get; }

        bool IsVirtual { get; }

        bool IsSecurity { get; }

        bool IsVisualIdentity { get; }
    }

    [DataContract]
    public abstract class PropertyMetadata<TType> : IPropertyMetadata, IEquatable<PropertyMetadata<TType>>
        where TType : IEquatable<TType>
    {
        protected PropertyMetadata(string name, TType type, bool isCollection, bool allowNull, bool isVirtual, bool isSecurity, bool isVisualIdentity)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.Type = type;
            this.IsCollection = isCollection;
            this.AllowNull = allowNull;
            this.IsVirtual = isVirtual;
            this.IsSecurity = isSecurity;
            this.IsVisualIdentity = isVisualIdentity;
        }


        public abstract TypeHeader TypeHeader { get; }


        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public TType Type { get; private set; }

        [DataMember]
        public bool IsCollection { get; private set; }

        [DataMember]
        public bool AllowNull { get; private set; }

        [DataMember]
        public bool IsVirtual { get; private set; }

        [DataMember]
        public bool IsSecurity { get; private set; }

        [DataMember]
        public bool IsVisualIdentity { get; private set; }



        public override string ToString()
        {
            return this.Name;
        }


        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.Type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PropertyMetadata<TType>);
        }

        public bool Equals(PropertyMetadata<TType> other)
        {
            return other != null && this.Name == other.Name
                                 && this.Type.Equals(other.Type)
                                 && this.IsCollection == other.IsCollection
                                 && this.AllowNull == other.AllowNull
                                 && this.IsVirtual == other.IsVirtual
                                 && this.IsSecurity == other.IsSecurity
                                 && this.IsVisualIdentity == other.IsVisualIdentity;
        }
    }
}
