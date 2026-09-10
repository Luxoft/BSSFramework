using System.Runtime.Serialization;

using Framework.Restriction;
using Framework.Validation.Attributes;

namespace SampleSystem.Domain.Inline;

[ExpandValidation]
[DataContract(Namespace = "")]
public class Fio : ICloneable, IEquatable<Fio>
{
    private string firstName = "";
    private string lastName = "";
    private string middleName = "";

    [MaxLength(50)]
    [DataMember]
    public string FirstName
    {
        get => this.firstName;
        set => this.firstName = value;
    }

    [MaxLength(50)]
    [DataMember]
    public string LastName
    {
        get => this.lastName;
        set => this.lastName = value;
    }

    [MaxLength(50)]
    [DataMember]
    public string MiddleName
    {
        get => this.middleName;
        set => this.middleName = value;
    }

    [DataMember]
    public string FullName
    {
        get => $"{this.LastName} {this.FirstName} {this.MiddleName}";
        private set
        {
        }
    }

    public override string ToString() => this.FullName;

    public Fio Clone() => (Fio)this.MemberwiseClone();

    object ICloneable.Clone() => this.Clone();

    public override bool Equals(object? obj) => this.Equals(obj as Fio);

    public override int GetHashCode() => 0;

    public bool Equals(Fio? other) =>
        other is not null
        && this.FirstName == other.FirstName
        && this.LastName == other.LastName
        && this.MiddleName == other.MiddleName;

    public static bool operator ==(Fio? v1, Fio? v2) => ReferenceEquals(v1, v2) || (v1 is not null && v1.Equals(v2));

    public static bool operator !=(Fio? v1, Fio? v2) => !(v1 == v2);
}
