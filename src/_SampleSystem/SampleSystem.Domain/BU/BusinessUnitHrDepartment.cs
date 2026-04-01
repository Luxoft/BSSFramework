using CommonFramework;

using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;
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
        get => this.businessUnit;
        set => this.businessUnit = value;
    }

    [Required]
    [UniqueElement]
    public virtual HRDepartment HRDepartment
    {
        get => this.hRDepartment;
        set => this.hRDepartment = value;
    }

    HRDepartment IDetail<HRDepartment>.Master => this.hRDepartment;
}
