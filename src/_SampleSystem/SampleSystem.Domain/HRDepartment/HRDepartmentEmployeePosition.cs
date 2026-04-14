using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.Employee;

namespace SampleSystem.Domain.HRDepartment;

public class HRDepartmentEmployeePosition : AuditPersistentDomainObjectBase, IDetail<HRDepartment>
{
    private EmployeePosition employeePosition;
    private HRDepartment hrDepartment;

    public HRDepartmentEmployeePosition(HRDepartment hrDepartment)
    {
        this.hrDepartment = hrDepartment;
        hrDepartment.AddDetail(this);
    }

    public HRDepartmentEmployeePosition()
    {
    }

    [UniqueElement]
    public virtual EmployeePosition EmployeePosition
    {
        get => this.employeePosition;
        set => this.employeePosition = value;
    }

    [UniqueElement]
    public virtual HRDepartment HrDepartment
    {
        get => this.hrDepartment;
        set => this.hrDepartment = value;
    }

    HRDepartment IDetail<HRDepartment>.Master => this.hrDepartment;
}
