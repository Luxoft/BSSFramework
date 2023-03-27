using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace SampleSystem.Domain;

[DomainType("77E78AEF-9512-46E0-A33D-AAE58DC7E18C")]
[BLLViewRole, BLLSaveRole(AllowCreate = false)]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.ManagementUnitView, SampleSystemSecurityOperationCode.EmployeeEdit)]
[SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.ManagementUnitEdit)]
public class ManagementUnit :
        CommonUnitBase,
        IDenormalizedHierarchicalPersistentSource<ManagementUnitAncestorLink, ManagementUnitToAncestorChildView, ManagementUnit, Guid>,
        IUnit<ManagementUnit>,
        IMaster<ManagementUnitAndBusinessUnitLink>,
        IMaster<ManagementUnitAndHRDepartmentLink>,
        IPeriodObject,
        ISecurityContext
{
    private readonly ICollection<ManagementUnit> children = new List<ManagementUnit>();

    private ICollection<ManagementUnitAndHRDepartmentLink> hRDepartments = new List<ManagementUnitAndHRDepartmentLink>();
    private ICollection<ManagementUnitAndBusinessUnitLink> businessUnits = new List<ManagementUnitAndBusinessUnitLink>();

    private ManagementUnit parent;

    private Period period;

    private bool isProduction;

    public virtual Period Period
    {
        get { return this.period; }
        set { this.period = value; }
    }

    public virtual bool IsProduction
    {
        get { return this.isProduction; }
        set { this.isProduction = value; }
    }

    [Framework.Restriction.UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndHRDepartmentLink> HRDepartments
    {
        get { return this.hRDepartments; }
    }

    [Framework.Restriction.UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndBusinessUnitLink> BusinessUnits
    {
        get { return this.businessUnits; }
    }

    /// <summary>
    ///  Supposed to be set from dto only.
    /// </summary>
    public virtual ManagementUnit Parent
    {
        get { return this.parent; }
        protected internal set { this.parent = value; }
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client | DTORole.Report)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
    public virtual IEnumerable<ManagementUnit> Children => this.children;

    ManagementUnit IUnit<ManagementUnit>.CurrentObject => this;

    ICollection<ManagementUnitAndHRDepartmentLink> IMaster<ManagementUnitAndHRDepartmentLink>.Details => (ICollection<ManagementUnitAndHRDepartmentLink>)this.HRDepartments;

    ICollection<ManagementUnitAndBusinessUnitLink> IMaster<ManagementUnitAndBusinessUnitLink>.Details => (ICollection<ManagementUnitAndBusinessUnitLink>)this.BusinessUnits;

    public static bool operator ==(ManagementUnit left, IUnit<ManagementUnit> right)
    {
        return object.Equals(left, right);
    }

    public static bool operator !=(ManagementUnit left, IUnit<ManagementUnit> right)
    {
        return !object.Equals(left, right);
    }
}
