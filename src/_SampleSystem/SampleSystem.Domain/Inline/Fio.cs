using System;
using System.Runtime.Serialization;

using Framework.Core;
using Framework.Validation;

namespace SampleSystem.Domain.Inline;

[ExpandValidation]
[DataContract(Namespace = "")]
public class Fio : FioShort, ICloneable<Fio>
{
    private string middleName;


    [Framework.Restriction.MaxLength(50)]
    [DataMember]
    public string MiddleName
    {
        get { return this.middleName.TrimNull(); }
        set { this.middleName = value.TrimNull(); }
    }

    public override string FullName
    {
        get { return $"{base.FullName} {this.MiddleName}"; }
    }

    public new Fio Clone()
    {
        return this.MemberwiseClone() as Fio;
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

    public bool Equals(Fio other)
    {
        return other != null
               && this.FirstName == other.FirstName
               && this.LastName == other.LastName
               && this.MiddleName == other.MiddleName;
    }

    public static bool operator ==(Fio v1, Fio v2)
    {
        return object.ReferenceEquals(v1, v2) || (!object.ReferenceEquals(v1, null) && v1.Equals(v2));
    }

    public static bool operator !=(Fio v1, Fio v2)
    {
        return !(v1 == v2);
    }
}
