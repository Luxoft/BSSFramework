using System;
using System.Runtime.Serialization;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.DomainDriven.SerializeMetadata;

[DataContract]
public class TypeHeader : IEquatable<TypeHeader>, IVisualIdentityObject
{
    public TypeHeader(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.Name = type.Name;
    }

    public TypeHeader(string name)
    {
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
    }


    [DataMember]
    public string Name { get; private set; }

    public virtual string GenerateName => this.Name;

    public override string ToString()
    {
        return this.Name;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as TypeHeader);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }

    public virtual bool Equals(TypeHeader other)
    {
        return !object.ReferenceEquals(other, null) && this.Name == other.Name;
    }


    public static bool operator ==(TypeHeader typeHeader, TypeHeader otherHeader)
    {
        return object.ReferenceEquals(typeHeader, otherHeader)

               || (!object.ReferenceEquals(typeHeader, null) && typeHeader.Equals(otherHeader));
    }

    public static bool operator !=(TypeHeader typeHeader, TypeHeader otherHeader)
    {
        return !(typeHeader == otherHeader);
    }
}
