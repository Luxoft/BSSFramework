using CommonFramework;

using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Relations;
using Framework.Restriction;
using Framework.Validation.Attributes;

using SampleSystem.Domain.BU.__Base;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Enums;
using SampleSystem.Domain.MU;
using SampleSystem.Domain.Projects;

using SecuritySystem;

namespace SampleSystem.Domain.BU;

[BLLViewRole, BLLSaveRole(AllowCreate = false)]
public partial class BusinessUnit :
        CommonUnitBase,
        IUnit<BusinessUnit>,
        IMaster<ManagementUnitAndBusinessUnitLink>,
        IMaster<BusinessUnitEmployeeRole>,
        IMaster<BusinessUnitManagerCommissionLink>,
        IMaster<BusinessUnit>,
        IMaster<Project>,
        IDetail<BusinessUnit>,
        ISecurityContext
{
    private readonly ICollection<BusinessUnitEmployeeRole> businessUnitEmployeeRoles = new List<BusinessUnitEmployeeRole>();
    private readonly ICollection<BusinessUnitManagerCommissionLink> managerCommissions = new List<BusinessUnitManagerCommissionLink>();

    private readonly ICollection<BusinessUnit> children = new List<BusinessUnit>();

    private readonly ICollection<Project> projects = new List<Project>();

    private DateTime? firstNewBusinessStatusMonth;
    private int newBusinessStatusLeft;
    private BusinessUnit? parent;
    private string projectStartMailList;
    private int rank;
    private DateTime? leastProjectStartDate;
    private DateTime? lastBusinessUnitHasNoLinkedProjectsWarningCheckDate;
    private bool needSendBusinessUnitHasNoLinkedProjectsWarning;
    private BusinessUnitType? businessUnitType;
    private BusinessUnitOptions options;

    private BusinessUnit businessUnitForRent;
    private decimal commission;
    private bool isNewBusiness;
    private bool isProduction;
    private Period period;

    private int deepLevel;

    private bool allowedForFilterRole;

    //private int order;

    public BusinessUnit()
    {
    }

    public BusinessUnit(BusinessUnit parent)
    {
        this.Parent = parent;
        if (null != parent)
        {
            parent.AddDetail(this);
        }
    }

    [UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndBusinessUnitLink> ManagementUnits { get; } = new List<ManagementUnitAndBusinessUnitLink>();

    public virtual bool AllowedForFilterRole
    {
        get => this.allowedForFilterRole;
        set => this.allowedForFilterRole = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual int DeepLevel
    {
        get => this.deepLevel;
        set => this.deepLevel = value;
    }

    public virtual DateTime? LastBusinessUnitHasNoLinkedProjectsWarningCheckDate
    {
        get => this.lastBusinessUnitHasNoLinkedProjectsWarningCheckDate;
        protected internal set => this.lastBusinessUnitHasNoLinkedProjectsWarningCheckDate = value;
    }

    public virtual bool NeedSendBusinessUnitHasNoLinkedProjectsWarning
    {
        get => this.needSendBusinessUnitHasNoLinkedProjectsWarning;
        protected internal set => this.needSendBusinessUnitHasNoLinkedProjectsWarning = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual BusinessUnitType? BusinessUnitType
    {
        get => this.businessUnitType;
        set => this.businessUnitType = value;
    }

    public virtual decimal Commission
    {
        get => this.commission;
        set => this.commission = value;
    }

    public virtual BusinessUnitOptions Options
    {
        get => this.options;
        set => this.options = value;
    }

    public virtual bool IsNewBusiness
    {
        get => this.isNewBusiness;
        set => this.isNewBusiness = value;
    }

    public virtual bool IsProduction
    {
        get => this.isProduction;
        set => this.isProduction = value;
    }

    public virtual bool IsSpecialCommission
    {
        get => this.options.HasFlag(BusinessUnitOptions.IsSpecialCommission);
        set
        {
            if (this.IsSpecialCommission != value)
            {
                this.options = this.options ^ BusinessUnitOptions.IsSpecialCommission;
            }
        }
    }

    public virtual bool IsPool
    {
        get => this.options.HasFlag(BusinessUnitOptions.IsResourcePool);
        set
        {
            if (this.IsPool != value)
            {
                this.options = this.options ^ BusinessUnitOptions.IsResourcePool;
            }
        }
    }

    public virtual BusinessUnit BusinessUnitForRent
    {
        get => this.businessUnitForRent;
        set => this.businessUnitForRent = value;
    }

    public virtual IEnumerable<BusinessUnitEmployeeRole> BusinessUnitEmployeeRoles => this.businessUnitEmployeeRoles;

    [UniqueGroup]
    [PropertyValidationMode(PropertyValidationMode.Auto, PropertyValidationMode.Disabled)]
    public virtual IEnumerable<BusinessUnitManagerCommissionLink> ManagerCommissions => this.managerCommissions;

    /// <summary>
    /// #IAD-20872
    /// </summary>
    public virtual DateTime? LeastProjectStartDate
    {
        get => this.leastProjectStartDate;
        set => this.leastProjectStartDate = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Period Period
    {
        get => this.period;
        set => this.period = value;
    }

    public virtual int Rank
    {
        get => this.rank;
        set => this.rank = value;
    }

    public virtual DateTime? FirstNewBusinessStatusMonth
    {
        get => this.firstNewBusinessStatusMonth;
        set => this.firstNewBusinessStatusMonth = value;
    }

    public virtual int NewBusinessStatusLeft
    {
        get => this.newBusinessStatusLeft;
        set => this.newBusinessStatusLeft = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual DateTime? LastNewBusinessStatusMonth
    {
        get
        {
            if (this.FirstNewBusinessStatusMonth == null)
            {
                return null;
            }

            if (this.NewBusinessStatusLeft <= 0)
            {
                return this.FirstNewBusinessStatusMonth.Value;
            }

            return this.FirstNewBusinessStatusMonth.Value.AddMonths(this.NewBusinessStatusLeft - 1);
        }
    }

    public virtual string BusinessUnitTypeName
    {
        get
        {
            if (this.BusinessUnitType == null)
            {
                return string.Empty;
            }

            return this.BusinessUnitType.Name;
        }
    }

    public virtual string ProjectStartMailList
    {
        get => this.projectStartMailList;
        set => this.projectStartMailList = value;
    }

    #region ForEventDTO

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit AccountOrDivision =>
        this.GetParentByTypeIds(new List<Guid>
                                {
                                    BusinessUnitType.AccountTypeId,
                                    BusinessUnitType.DivisionTypeId
                                });

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit LobOrService =>
        this.GetParentByTypeIds(new List<Guid>
                                {
                                    BusinessUnitType.LobTypeId,
                                    BusinessUnitType.ServiceTypeId
                                });

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit Account =>
        this.GetParentByTypeIds(new List<Guid>
                                {
                                    BusinessUnitType.AccountTypeId,
                                });

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit Program =>
        this.GetParentByTypeIds(new List<Guid>
                                {
                                    BusinessUnitType.ProgramTypeId,
                                });

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit Lob =>
        this.GetParentByTypeIds(new List<Guid>
                                {
                                    BusinessUnitType.LobTypeId,
                                });

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string AccountOrDivisionName => this.AccountOrDivision.Maybe(z => z.Name);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string LobOrServiceName => this.LobOrService.Maybe(z => z.Name);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string ProgramName => this.Program.Maybe(z => z.Name);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string AccountName => this.Account.Maybe(z => z.Name);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration)]
    public virtual string LobName => this.Lob.Maybe(z => z.Name);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid AccountOrDivisionId => this.AccountOrDivision.Maybe(z => z.Id);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid LobOrServiceId => this.LobOrService.Maybe(z => z.Id);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid AccountId => this.Account.Maybe(z => z.Id);

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid LobId => this.Lob.Maybe(z => z.Id);

    #endregion

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration | DTORole.Event)]
    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client)]
    public virtual IEnumerable<BusinessUnit> Children => this.children;

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual IEnumerable<Project> Projects => this.projects;

    /// <summary>
    /// Supposed to be set from dto only.
    /// </summary>
    [IsMaster]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual BusinessUnit? Parent
    {
        get => this.parent;
        set => this.parent = value;
    }

    BusinessUnit IUnit<BusinessUnit>.CurrentObject => this;

    ICollection<BusinessUnitEmployeeRole> IMaster<BusinessUnitEmployeeRole>.Details => (ICollection<BusinessUnitEmployeeRole>)this.BusinessUnitEmployeeRoles;

    ICollection<ManagementUnitAndBusinessUnitLink> IMaster<ManagementUnitAndBusinessUnitLink>.Details => (ICollection<ManagementUnitAndBusinessUnitLink>)this.ManagementUnits;

    ICollection<BusinessUnitManagerCommissionLink> IMaster<BusinessUnitManagerCommissionLink>.Details => (ICollection<BusinessUnitManagerCommissionLink>)this.ManagerCommissions;

    ICollection<BusinessUnit> IMaster<BusinessUnit>.Details => (ICollection<BusinessUnit>)this.Children;

    BusinessUnit IDetail<BusinessUnit>.Master => this.Parent;

    ICollection<Project> IMaster<Project>.Details => (ICollection<Project>)this.Projects;

    public static bool operator ==(BusinessUnit left, IUnit<BusinessUnit> right) => Equals(left, right);

    public static bool operator !=(BusinessUnit left, IUnit<BusinessUnit> right) => !Equals(left, right);

    public override string ToString() => $"{base.ToString()}";

    public virtual BusinessUnit? GetParentByTypeIds(IEnumerable<Guid> businessUnitTypesIds) =>
        businessUnitTypesIds.Contains(this.BusinessUnitType.Maybe(v => v.Id))
            ? this
            : this.Parent.Maybe(z => z.GetParentByTypeIds(businessUnitTypesIds));
}
