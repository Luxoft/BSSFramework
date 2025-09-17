using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
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

    [Required]
    [UniqueElement]
    public virtual BusinessUnit BusinessUnit
    {
        get { return this.businessUnit; }
        set { this.businessUnit = value; }
    }

    [Required]
    [UniqueElement]
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
