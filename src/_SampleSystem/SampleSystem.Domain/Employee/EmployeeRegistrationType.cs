using Framework.DomainDriven.BLL;
using Framework.Restriction;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLViewRole]
[UniqueGroup]
[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.Disabled))]
public class EmployeeRegistrationType : BaseDirectory, IExternalSynchronizable
{
    private long externalId;

    public virtual long ExternalId
    {
        get { return this.externalId; }
        set { this.externalId = value; }
    }
}
