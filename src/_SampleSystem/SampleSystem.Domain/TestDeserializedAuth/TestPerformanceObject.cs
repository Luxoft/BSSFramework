using Framework.BLL.Domain.ServiceRole;

namespace SampleSystem.Domain;

[BLLViewRole]
public class TestPerformanceObject : BaseDirectory
{
    private Location location;

    private Employee employee;

    private BusinessUnit businessUnit;

    private ManagementUnit managementUnit;

    public virtual Location Location
    {
        get => this.location;
        set => this.location = value;
    }

    public virtual Employee Employee
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
