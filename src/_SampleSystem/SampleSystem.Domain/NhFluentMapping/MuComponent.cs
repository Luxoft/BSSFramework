using Framework.BLL.Domain.Serialization;

namespace SampleSystem.Domain.NhFluentMapping;

public class MuComponent
{
    private bool? luxoftSignsFirst;

    private Employee.Employee authorizedLuxoftSignatory;

    public virtual bool? LuxoftSignsFirst
    {
        get => this.luxoftSignsFirst;
        set => this.luxoftSignsFirst = value;
    }

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual Employee.Employee AuthorizedLuxoftSignatory
    {
        get => this.authorizedLuxoftSignatory;
        set => this.authorizedLuxoftSignatory = value;
    }
}
