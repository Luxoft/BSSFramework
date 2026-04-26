using Anch.Core;

using Framework.Relations;
using Framework.Restriction;

namespace SampleSystem.Domain.Employee.EmpoloyeeLink;

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
            : this(employee) =>
        this.specialization = specialization;

    protected EmployeeAndEmployeeSpecializationLink()
    {
    }

    [Required]
    [UniqueElement]
    public virtual EmployeeSpecialization Specialization
    {
        get => this.specialization;
        set => this.specialization = value;
    }

    [Required]
    [UniqueElement]
    public virtual Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }

    Employee IDetail<Employee>.Master => this.Employee;
}
