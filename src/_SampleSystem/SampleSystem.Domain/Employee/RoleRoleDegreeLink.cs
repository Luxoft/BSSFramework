using Framework.DomainDriven.BLL;
using Framework.Restriction;
using Framework.Validation;

namespace SampleSystem.Domain;

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
        get { return this.roleDegree; }
        set { this.roleDegree = value; }
    }

    [UniqueElement("UniLink")]
    public virtual EmployeeRole Role
    {
        get { return this.role; }
        set { this.role = value; }
    }

    public virtual EmployeeRole AnotherRole
    {
        get { return this.anotherRole; }
        set { this.anotherRole = value;  }
    }
}
