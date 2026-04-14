using CommonFramework;

using Framework.Application.Domain;
using Framework.BLL.Domain.Persistent.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.BU;

namespace SampleSystem.Domain.MU;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
public class ManagementUnitAndHRDepartmentLink :
        AuditPersistentDomainObjectBase,
        IDetail<ManagementUnit>,
        IDetail<HRDepartment.HRDepartment>,
        IVisualIdentityObject
{
    private HRDepartment.HRDepartment hRDepartment;
    private ManagementUnit managementUnit;

    public ManagementUnitAndHRDepartmentLink(ManagementUnit managementUnit)
    {
        this.managementUnit = managementUnit;
        this.managementUnit.Maybe(z => z.AddDetail(this));
    }

    public ManagementUnitAndHRDepartmentLink(HRDepartment.HRDepartment hRDepartment)
    {
        this.hRDepartment = hRDepartment;
        this.hRDepartment.Maybe(z => z.AddDetail(this));
    }

    public ManagementUnitAndHRDepartmentLink(ManagementUnit managementUnit, HRDepartment.HRDepartment hRDepartment)
            : this(managementUnit) =>
        this.hRDepartment = hRDepartment;

    public ManagementUnitAndHRDepartmentLink()
    {
    }

    [UniqueElement]
    public virtual HRDepartment.HRDepartment HRDepartment
    {
        get => this.hRDepartment;
        set => this.hRDepartment = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Event | DTORole.Integration)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client)]
    [ExpandPath("ManagementUnit.BusinessUnits")]
    [DetailRole(false)]
    public virtual IEnumerable<BusinessUnit> LinkedBusinessUnits => this.ManagementUnit.BusinessUnits.ToList(link => link.BusinessUnit);

    [UniqueElement]
    public virtual ManagementUnit ManagementUnit
    {
        get => this.managementUnit;
        set => this.managementUnit = value;
    }

    ManagementUnit IDetail<ManagementUnit>.Master => this.managementUnit;

    HRDepartment.HRDepartment IDetail<HRDepartment.HRDepartment>.Master => this.hRDepartment;

    string IVisualIdentityObject.Name => this.HRDepartment.Maybe(x => x.Name) + "-" + this.ManagementUnit.Maybe(x => x.Name);
}
