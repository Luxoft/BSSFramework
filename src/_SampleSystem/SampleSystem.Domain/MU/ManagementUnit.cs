using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.BU.__Base;

using SecuritySystem;

namespace SampleSystem.Domain.MU;

[BLLViewRole, BLLSaveRole(AllowCreate = false)]
public class ManagementUnit :
        CommonUnitBase,
        IUnit<ManagementUnit>,
        IMaster<ManagementUnitAndBusinessUnitLink>,
        IMaster<ManagementUnitAndHRDepartmentLink>,
        IPeriodObject,
        ISecurityContext
{
    private readonly ICollection<ManagementUnit> children = new List<ManagementUnit>();

    private ICollection<ManagementUnitAndHRDepartmentLink> hRDepartments = new List<ManagementUnitAndHRDepartmentLink>();
    private ICollection<ManagementUnitAndBusinessUnitLink> businessUnits = new List<ManagementUnitAndBusinessUnitLink>();

    private ManagementUnit? parent;

    private Period period;

    private bool isProduction;

    private int deepLevel;


    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual int DeepLevel
    {
        get => this.deepLevel;
        set => this.deepLevel = value;
    }

    public virtual Period Period
    {
        get => this.period;
        set => this.period = value;
    }

    public virtual bool IsProduction
    {
        get => this.isProduction;
        set => this.isProduction = value;
    }

    [UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndHRDepartmentLink> HRDepartments => this.hRDepartments;

    [UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndBusinessUnitLink> BusinessUnits => this.businessUnits;

    /// <summary>
    ///  Supposed to be set from dto only.
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual ManagementUnit? Parent
    {
        get => this.parent;
        set => this.parent = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client | DTORole.Report)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
    public virtual IEnumerable<ManagementUnit> Children => this.children;

    ManagementUnit IUnit<ManagementUnit>.CurrentObject => this;

    ICollection<ManagementUnitAndHRDepartmentLink> IMaster<ManagementUnitAndHRDepartmentLink>.Details => (ICollection<ManagementUnitAndHRDepartmentLink>)this.HRDepartments;

    ICollection<ManagementUnitAndBusinessUnitLink> IMaster<ManagementUnitAndBusinessUnitLink>.Details => (ICollection<ManagementUnitAndBusinessUnitLink>)this.BusinessUnits;

    public static bool operator ==(ManagementUnit left, IUnit<ManagementUnit> right) => Equals(left, right);

    public static bool operator !=(ManagementUnit left, IUnit<ManagementUnit> right) => !Equals(left, right);
}
