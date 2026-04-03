using Framework.BLL.Domain.Attributes.Round;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.Serialization;

using SampleSystem.Domain.Inline;

namespace SampleSystem.Domain;

public class InsuranceDetail : AuditPersistentDomainObjectBase
{
    private decimal cost;

    private Fio fio;

    private DateTime? birthDate;

    private int age;

    private string landlinePhone;

    private string cellPhone;

    private string registrationAddress;

    private string residentalAddress;

    [Money]
    public virtual decimal Cost
    {
        get => this.cost.RoundMoney();
        set => this.cost = value.RoundMoney();
    }

    public virtual Fio Fio
    {
        get => this.fio;
        set => this.fio = value;
    }

    public virtual DateTime? BirthDate
    {
        get => this.birthDate;
        set => this.birthDate = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual int Age
    {
        get => this.age;
        set => this.age = value;
    }

    public virtual string LandlinePhone
    {
        get => this.landlinePhone;
        set => this.landlinePhone = value;
    }

    public virtual string CellPhone
    {
        get => this.cellPhone;
        set => this.cellPhone = value;
    }

    public virtual string RegistrationAddress
    {
        get => this.registrationAddress;
        set => this.registrationAddress = value;
    }

    public virtual string ResidentalAddress
    {
        get => this.residentalAddress;
        set => this.residentalAddress = value;
    }
}
