using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain;

[BLLViewRole]
public class TestRestrictionObject : AuditPersistentDomainObjectBase
{
    private bool restrictionHandler;

    private BusinessUnit businessUnit;

    public virtual bool RestrictionHandler
    {
        get { return this.restrictionHandler; }
        set { this.restrictionHandler = value; }
    }

    public virtual BusinessUnit BusinessUnit
    {
        get { return this.businessUnit; }
        set { this.businessUnit = value; }
    }
}
