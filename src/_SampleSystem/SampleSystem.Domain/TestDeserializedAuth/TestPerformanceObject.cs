using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain;

[BLLViewRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeeView)]
public class TestPerformanceObject : BaseDirectory
{
    private Location location;

    private Employee employee;

    private BusinessUnit businessUnit;

    private ManagementUnit managementUnit;

    public virtual Location Location
    {
        get { return this.location; }
        set { this.location = value; }
    }

    public virtual Employee Employee
    {
        get { return this.employee; }
        set { this.employee = value; }
    }
    public virtual BusinessUnit BusinessUnit
    {
        get { return this.businessUnit; }
        set { this.businessUnit = value; }
    }
    public virtual ManagementUnit ManagementUnit
    {
        get { return this.managementUnit; }
        set { this.managementUnit = value; }
    }
}
