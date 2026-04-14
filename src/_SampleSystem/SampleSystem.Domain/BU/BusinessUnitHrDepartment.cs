using CommonFramework;

using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;
using Framework.Restriction;

namespace SampleSystem.Domain.BU;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
public class BusinessUnitHrDepartment : AuditPersistentDomainObjectBase, IDetail<HRDepartment.HRDepartment>
{
    private BusinessUnit businessUnit;
    private HRDepartment.HRDepartment hRDepartment;

    public BusinessUnitHrDepartment(HRDepartment.HRDepartment hrDepartment)
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
        get => this.businessUnit;
        set => this.businessUnit = value;
    }

    [Required]
    [UniqueElement]
    public virtual HRDepartment.HRDepartment HRDepartment
    {
        get => this.hRDepartment;
        set => this.hRDepartment = value;
    }

    HRDepartment.HRDepartment IDetail<HRDepartment.HRDepartment>.Master => this.hRDepartment;
}
