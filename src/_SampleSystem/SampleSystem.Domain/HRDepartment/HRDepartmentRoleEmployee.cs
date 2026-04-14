using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.Enums;

namespace SampleSystem.Domain.HRDepartment;

public class HRDepartmentRoleEmployee : AuditPersistentDomainObjectBase, IDetail<HRDepartment>
{
    private Employee.Employee employee;
    private HRDepartment hRDepartment;
    private HRDepartmentEmployeeRoleType hRDepartmentEmployeeRoleType;

    public HRDepartmentRoleEmployee(HRDepartment hRDepartment)
    {
        this.hRDepartment = hRDepartment;
        hRDepartment.AddDetail(this);
    }

    protected HRDepartmentRoleEmployee()
    {
    }

    [Required]
    [UniqueElement]
    public virtual Employee.Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }

    public virtual HRDepartment HRDepartment => this.hRDepartment;

    [UniqueElement]
    [Required]
    public virtual HRDepartmentEmployeeRoleType HRDepartmentEmployeeRoleType
    {
        get => this.hRDepartmentEmployeeRoleType;
        set => this.hRDepartmentEmployeeRoleType = value;
    }

    HRDepartment IDetail<HRDepartment>.Master => this.hRDepartment;
}
