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

[BLLViewRole, BLLRemoveRole, BLLSaveRole]
public class ManagementUnitAndBusinessUnitLink :
        AuditPersistentDomainObjectBase,
        IDetail<ManagementUnit>,
        IDetail<BusinessUnit>,
        IVisualIdentityObject
{
    private BusinessUnit businessUnit;
    private ManagementUnit managementUnit;
    private bool equalBU;

    public ManagementUnitAndBusinessUnitLink(ManagementUnit managementUnit)
    {
        this.managementUnit = managementUnit;
        this.managementUnit.Maybe(z => z.AddDetail(this));
    }

    public ManagementUnitAndBusinessUnitLink(ManagementUnit managementUnit, BusinessUnit businessUnit)
            : this(managementUnit) =>
        this.businessUnit = businessUnit;

    public ManagementUnitAndBusinessUnitLink(BusinessUnit businessUnit)
    {
        this.businessUnit = businessUnit;
        this.businessUnit.Maybe(z => z.AddDetail(this));
    }

    public ManagementUnitAndBusinessUnitLink(BusinessUnit businessUnit, ManagementUnit managementUnit)
            : this(businessUnit) =>
        this.managementUnit = managementUnit;

    public ManagementUnitAndBusinessUnitLink()
    {
    }

    public virtual bool EqualBU
    {
        get => this.equalBU;
        set => this.equalBU = value;
    }

    [Required]
    [UniqueElement]
    public virtual ManagementUnit ManagementUnit
    {
        get => this.managementUnit;
        set => this.managementUnit = value;
    }

    [Required]
    [UniqueElement]
    public virtual BusinessUnit BusinessUnit
    {
        get => this.businessUnit;
        set => this.businessUnit = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Event | DTORole.Integration)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client)]
    [ExpandPath("ManagementUnit.HRDepartments")]
    [DetailRole(false)]
    public virtual IEnumerable<HRDepartment.HRDepartment> LinkedHRDepartments => this.ManagementUnit.HRDepartments.ToList(link => link.HRDepartment);

    ManagementUnit IDetail<ManagementUnit>.Master => this.managementUnit;

    BusinessUnit IDetail<BusinessUnit>.Master => this.businessUnit;

    string IVisualIdentityObject.Name => this.BusinessUnit.Maybe(x => x.Name) + "-" + this.ManagementUnit.Maybe(x => x.Name);
}
