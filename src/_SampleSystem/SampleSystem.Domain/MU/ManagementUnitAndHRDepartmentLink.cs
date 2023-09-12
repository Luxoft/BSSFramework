using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkView))]
[EditDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkEdit))]
public class ManagementUnitAndHRDepartmentLink :
        AuditPersistentDomainObjectBase,
        IDetail<ManagementUnit>,
        IDetail<HRDepartment>,
        IVisualIdentityObject
{
    private HRDepartment hRDepartment;
    private ManagementUnit managementUnit;

    public ManagementUnitAndHRDepartmentLink(ManagementUnit managementUnit)
    {
        this.managementUnit = managementUnit;
        this.managementUnit.Maybe(z => z.AddDetail(this));
    }

    public ManagementUnitAndHRDepartmentLink(HRDepartment hRDepartment)
    {
        this.hRDepartment = hRDepartment;
        this.hRDepartment.Maybe(z => z.AddDetail(this));
    }

    public ManagementUnitAndHRDepartmentLink(ManagementUnit managementUnit, HRDepartment hRDepartment)
            : this(managementUnit)
    {
        this.hRDepartment = hRDepartment;
    }

    public ManagementUnitAndHRDepartmentLink()
    {
    }

    [UniqueElement]
    public virtual HRDepartment HRDepartment
    {
        get
        {
            return this.hRDepartment;
        }

        set
        {
            this.hRDepartment = value;
        }
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Event | DTORole.Integration)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client)]
    [ExpandPath("ManagementUnit.BusinessUnits")]
    [DetailRole(false)]
    public virtual IEnumerable<BusinessUnit> LinkedBusinessUnits
    {
        get
        {
            return this.ManagementUnit.BusinessUnits.ToList(link => link.BusinessUnit);
        }
    }

    [UniqueElement]
    public virtual ManagementUnit ManagementUnit
    {
        get
        {
            return this.managementUnit;
        }

        set
        {
            this.managementUnit = value;
        }
    }

    ManagementUnit IDetail<ManagementUnit>.Master
    {
        get
        {
            return this.managementUnit;
        }
    }

    HRDepartment IDetail<HRDepartment>.Master
    {
        get
        {
            return this.hRDepartment;
        }
    }

    string IVisualIdentityObject.Name
    {
        get
        {
            return this.HRDepartment.Maybe(x => x.Name) + "-" + this.ManagementUnit.Maybe(x => x.Name);
        }
    }
}
