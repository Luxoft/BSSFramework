using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[UniqueGroup]
public class EmployeeRegistrationType : BaseDirectory, IExternalSynchronizable
{
    private long externalId;

    public virtual long ExternalId
    {
        get { return this.externalId; }
        set { this.externalId = value; }
    }
}
