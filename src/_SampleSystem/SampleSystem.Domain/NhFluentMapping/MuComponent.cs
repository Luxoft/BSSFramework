using Framework.DomainDriven.Serialization;

namespace SampleSystem.Domain;

public class MuComponent
{
    private bool? luxoftSignsFirst;

    private Employee authorizedLuxoftSignatory;

    public virtual bool? LuxoftSignsFirst
    {
        get => this.luxoftSignsFirst;
        set => this.luxoftSignsFirst = value;
    }

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual Employee AuthorizedLuxoftSignatory
    {
        get => this.authorizedLuxoftSignatory;
        set => this.authorizedLuxoftSignatory = value;
    }
}
