namespace SampleSystem.Domain;

public abstract class CommonUnitBase : BaseDirectory
{
    private BusinessUnitStatus businessUnitStatus;

    public virtual BusinessUnitStatus BusinessUnitStatus
    {
        get { return this.businessUnitStatus; }
        set { this.businessUnitStatus = value; }
    }
}
