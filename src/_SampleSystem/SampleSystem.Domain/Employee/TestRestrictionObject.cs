using Framework.BLL.Domain.ServiceRole;

namespace SampleSystem.Domain;

[BLLViewRole]
public class TestRestrictionObject : AuditPersistentDomainObjectBase
{
    private bool restrictionHandler;

    private BusinessUnit businessUnit;

    public virtual bool RestrictionHandler
    {
        get => this.restrictionHandler;
        set => this.restrictionHandler = value;
    }

    public virtual BusinessUnit BusinessUnit
    {
        get => this.businessUnit;
        set => this.businessUnit = value;
    }
}
