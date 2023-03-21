using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.BusinessUnitHrDepartmentView)]
[SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.BusinessUnitHrDepartmentEdit)]
public class BusinessUnitHrDepartment : AuditPersistentDomainObjectBase, IDetail<HRDepartment>
{
    private BusinessUnit businessUnit;
    private HRDepartment hRDepartment;

    public BusinessUnitHrDepartment(HRDepartment hrDepartment)
    {
        this.hRDepartment = hrDepartment;
        this.hRDepartment.Maybe(z => z.AddDetail(this));
    }

    public BusinessUnitHrDepartment()
    {
    }

    [Framework.Restriction.Required]
    [Framework.Restriction.UniqueElement]
    public virtual BusinessUnit BusinessUnit
    {
        get { return this.businessUnit; }
        set { this.businessUnit = value; }
    }

    [Framework.Restriction.Required]
    [Framework.Restriction.UniqueElement]
    public virtual HRDepartment HRDepartment
    {
        get { return this.hRDepartment; }
        set { this.hRDepartment = value; }
    }

    HRDepartment IDetail<HRDepartment>.Master
    {
        get { return this.hRDepartment; }
    }
}
