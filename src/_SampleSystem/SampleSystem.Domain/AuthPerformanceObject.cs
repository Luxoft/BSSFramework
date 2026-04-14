using SampleSystem.Domain.BU;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.MU;

namespace SampleSystem.Domain;

public class AuthPerformanceObject : AuditPersistentDomainObjectBase
{
    private BusinessUnit? businessUnit;

    private ManagementUnit? managementUnit;

    private Location? location;

    private Employee.Employee? employee;


    public virtual BusinessUnit? BusinessUnit { get => this.businessUnit; set => this.businessUnit = value; }

    public virtual ManagementUnit? ManagementUnit { get => this.managementUnit; set => this.managementUnit = value; }

    public virtual Location? Location { get => this.location; set => this.location = value; }

    public virtual Employee.Employee? Employee { get => this.employee; set => this.employee = value; }
}
