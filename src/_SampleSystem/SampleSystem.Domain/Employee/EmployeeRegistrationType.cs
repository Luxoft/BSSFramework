using Framework.BLL.Domain.ServiceRole;
using Framework.Restriction;

namespace SampleSystem.Domain.Employee;

[BLLViewRole]
[UniqueGroup]
public class EmployeeRegistrationType : BaseDirectory, IExternalSynchronizable
{
    private long externalId;

    public virtual long ExternalId
    {
        get => this.externalId;
        set => this.externalId = value;
    }
}
