using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain;

public class EmployeeAndEmployeeSpecializationLink : AuditPersistentDomainObjectBase, IDetail<Employee>
{
    private Employee employee;
    private EmployeeSpecialization specialization;

    public EmployeeAndEmployeeSpecializationLink(Employee employee)
    {
        this.employee = employee;
        this.employee.Maybe(z => z.AddDetail(this));
    }

    public EmployeeAndEmployeeSpecializationLink(Employee employee, EmployeeSpecialization specialization)
            : this(employee)
    {
        this.specialization = specialization;
    }

    protected EmployeeAndEmployeeSpecializationLink()
    {
    }

    [Required]
    [UniqueElement]
    public virtual EmployeeSpecialization Specialization
    {
        get { return this.specialization; }
        set { this.specialization = value; }
    }

    [Required]
    [UniqueElement]
    public virtual Employee Employee
    {
        get { return this.employee; }
        set { this.employee = value; }
    }

    Employee IDetail<Employee>.Master
    {
        get { return this.Employee; }
    }
}
