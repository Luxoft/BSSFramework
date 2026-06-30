using Framework.BLL.Domain.ServiceRole;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.MU;

namespace SampleSystem.Domain.TestDeserializedAuth;

[BLLViewRole]
public class TestPerformanceObject : BaseDirectory
{
    private Location location = null!;

    private Employee.Employee employee = null!;

    private BusinessUnit businessUnit = null!;

    private ManagementUnit managementUnit = null!;

    public virtual Location Location
    {
        get => this.location;
        set => this.location = value;
    }

    public virtual Employee.Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }
    public virtual BusinessUnit BusinessUnit
    {
        get => this.businessUnit;
        set => this.businessUnit = value;
    }
    public virtual ManagementUnit ManagementUnit
    {
        get => this.managementUnit;
        set => this.managementUnit = value;
    }
}

