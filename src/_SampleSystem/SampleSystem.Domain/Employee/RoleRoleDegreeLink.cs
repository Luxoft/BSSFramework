using Framework.BLL.Domain.ServiceRole.Base;
using Framework.Restriction;
using Framework.Validation;

namespace SampleSystem.Domain.Employee;

[BLLRole]
[UniqueGroup("UniLink")]
[CustomName("Role-Seniority link")]
public class RoleRoleDegreeLink : AuditPersistentDomainObjectBase
{
    private EmployeeRoleDegree roleDegree;

    private EmployeeRole role;

    private EmployeeRole anotherRole;

    [UniqueElement("UniLink")]
    [CustomName("Seniority")]
    public virtual EmployeeRoleDegree RoleDegree
    {
        get => this.roleDegree;
        set => this.roleDegree = value;
    }

    [UniqueElement("UniLink")]
    public virtual EmployeeRole Role
    {
        get => this.role;
        set => this.role = value;
    }

    public virtual EmployeeRole AnotherRole
    {
        get => this.anotherRole;
        set => this.anotherRole = value;
    }
}
