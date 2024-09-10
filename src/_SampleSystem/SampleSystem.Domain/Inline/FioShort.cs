using System.Runtime.Serialization;

using Framework.DomainDriven.Serialization;
using Framework.Core;
using Framework.Restriction;
using Framework.Validation;

namespace SampleSystem.Domain.Inline;

[ExpandValidation]
[DataContract(Namespace = "")]
[KnownType(typeof(Fio))]
public class FioShort : ICloneable<FioShort>, IEquatable<FioShort>
{
    private string firstName;
    private string lastName;


    public FioShort()
    {

    }



    [MaxLength(50)]
    [DataMember]
    public string FirstName
    {
        get { return this.firstName.TrimNull(); }
        set { this.firstName = value.TrimNull(); }
    }


    [MaxLength(50)]
    [DataMember]
    public string LastName
    {
        get { return this.lastName.TrimNull(); }
        set { this.lastName = value.TrimNull(); }
    }


    //[MaxLength(50)]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    [DataMember]
    public virtual string FullName
    {
        get { return $"{this.LastName} {this.FirstName}"; }
        private set
        {
            //throw new ApplicationException("Cannot be set");
        }
    }

    public override string ToString()
    {
        return this.FullName;
    }

    public FioShort Clone ()
    {
        return this.MemberwiseClone() as FioShort;
    }

    object ICloneable.Clone()
    {
        return this.Clone();
    }


    public override bool Equals(object obj)
    {
        return this.Equals(obj as FioShort);
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public bool Equals(FioShort other)
    {
        return other != null
               && this.FirstName == other.FirstName
               && this.LastName == other.LastName;
    }

    public static bool operator ==(FioShort v1, FioShort v2)
    {
        return ReferenceEquals(v1, v2) || (!ReferenceEquals(v1, null) && v1.Equals(v2));
    }

    public static bool operator !=(FioShort v1, FioShort v2)
    {
        return !(v1 == v2);
    }
}
