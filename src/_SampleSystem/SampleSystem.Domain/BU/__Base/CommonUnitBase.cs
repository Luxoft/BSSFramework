using SampleSystem.Domain.Enums;

namespace SampleSystem.Domain.BU.__Base;

public abstract class CommonUnitBase : BaseDirectory
{
    private BusinessUnitStatus businessUnitStatus;

    public virtual BusinessUnitStatus BusinessUnitStatus
    {
        get => this.businessUnitStatus;
        set => this.businessUnitStatus = value;
    }
}
