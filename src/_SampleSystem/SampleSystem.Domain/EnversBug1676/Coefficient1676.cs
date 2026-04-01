namespace SampleSystem.Domain.EnversBug1676;

public class Coefficient1676 : AuditPersistentDomainObjectBase
{
    private Location1676 location;

    private decimal normCoefficient;

    public virtual Location1676 Location
    {
        get => this.location;
        set => this.location = value;
    }

    public virtual decimal NormCoefficient
    {
        get => this.normCoefficient;
        set => this.normCoefficient = value;
    }
}
