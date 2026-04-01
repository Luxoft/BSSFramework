using CommonFramework;

using Framework.Relations;
using Framework.Restriction;

namespace SampleSystem.Domain;

public class BusinessUnitEmployeeRole : AuditPersistentDomainObjectBase, IDetail<BusinessUnit>
{
    private BusinessUnit businessUnit;
    private Employee employee;
    private BusinessUnitEmployeeRoleType role;

    public BusinessUnitEmployeeRole(BusinessUnit businessUnit)
    {
        if (businessUnit == null) throw new ArgumentNullException(nameof(businessUnit));

        this.businessUnit = businessUnit;
        this.businessUnit.Maybe(z => z.AddDetail(this));
    }

    protected BusinessUnitEmployeeRole()
    {
    }

    [Required]
    public virtual BusinessUnit BusinessUnit => this.businessUnit;

    [Required]
    public virtual Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }

    public virtual BusinessUnitEmployeeRoleType Role
    {
        get => this.role;
        set => this.role = value;
    }

    BusinessUnit IDetail<BusinessUnit>.Master => this.businessUnit;

    public static BusinessUnitEmployeeRole Create(
            BusinessUnit businessUnit,
            Employee employee,
            BusinessUnitEmployeeRoleType role)
    {
        if (businessUnit == null)
        {
            throw new ArgumentNullException(nameof(businessUnit));
        }

        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        return new BusinessUnitEmployeeRole(businessUnit)
               {
                       Employee = employee,
                       Role = role
               };
    }
}
