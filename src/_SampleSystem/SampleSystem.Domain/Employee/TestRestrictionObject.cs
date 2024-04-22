using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain;

[BLLViewRole]
[DomainType("{1A7C78AD-9371-4DAF-895C-EF6E5A8A0350}")]
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
