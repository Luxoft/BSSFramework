namespace SampleSystem.Domain;

public abstract class CommonUnitBase : ContextBaseDirectory
{
    private BusinessUnitStatus businessUnitStatus;

    public virtual BusinessUnitStatus BusinessUnitStatus
    {
        get { return this.businessUnitStatus; }
        set { this.businessUnitStatus = value; }
    }
}
