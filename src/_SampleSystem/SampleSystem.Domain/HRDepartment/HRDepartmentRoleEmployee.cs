using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain;

public class HRDepartmentRoleEmployee : AuditPersistentDomainObjectBase, IDetail<HRDepartment>
{
    private Employee employee;
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
    public virtual Employee Employee
    {
        get { return this.employee; }
        set { this.employee = value; }
    }

    public virtual HRDepartment HRDepartment
    {
        get { return this.hRDepartment; }
    }

    [UniqueElement]
    [Required]
    public virtual HRDepartmentEmployeeRoleType HRDepartmentEmployeeRoleType
    {
        get { return this.hRDepartmentEmployeeRoleType; }
        set { this.hRDepartmentEmployeeRoleType = value; }
    }

    HRDepartment IDetail<HRDepartment>.Master
    {
        get { return this.hRDepartment; }
    }
}
