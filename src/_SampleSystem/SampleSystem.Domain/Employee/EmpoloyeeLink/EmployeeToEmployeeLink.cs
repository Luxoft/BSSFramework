using CommonFramework;

using Framework.Relations;

using SampleSystem.Domain.Enums;

namespace SampleSystem.Domain.Employee.EmpoloyeeLink;

public class EmployeeToEmployeeLink : AuditPersistentDomainObjectBase, IDetail<Employee>
{
    private EmployeeLinkType employeeLinkType;
    private Employee linkedEmployee;
    private Employee owner;

    public EmployeeToEmployeeLink(Employee owner)
    {
        this.owner = owner;
        this.owner.Maybe(z => z.AddDetail(this));
    }

    public EmployeeToEmployeeLink(Employee owner, Employee linkedEmployee)
            : this(owner) =>
        this.linkedEmployee = linkedEmployee;

    public EmployeeToEmployeeLink(Employee owner, Employee linkedEmployee, EmployeeLinkType employeeLinkType)
            : this(owner, linkedEmployee) =>
        this.employeeLinkType = employeeLinkType;

    protected EmployeeToEmployeeLink()
    {
    }

    [IsMaster]
    public virtual Employee Owner => this.owner;

    public virtual Employee LinkedEmployee
    {
        get => this.linkedEmployee;
        set => this.linkedEmployee = value;
    }

    public virtual EmployeeLinkType EmployeeLinkType
    {
        get => this.employeeLinkType;
        set => this.employeeLinkType = value;
    }


    Employee IDetail<Employee>.Master => this.owner;
}
