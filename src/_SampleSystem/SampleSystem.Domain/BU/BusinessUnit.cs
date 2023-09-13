using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Validation;

namespace SampleSystem.Domain;

[DomainType("5C326B10-B4B4-402C-BCCE-A311016CB715")]
[BLLViewRole, BLLSaveRole(AllowCreate = false)]
[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.BusinessUnitView), nameof(SampleSystemSecurityOperation.BusinessUnitHrDepartmentView), SourceTypes = new[] { typeof(Employee), typeof(BusinessUnitHrDepartment) })]
[EditDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.BusinessUnitEdit))]
public partial class BusinessUnit :
        CommonUnitBase,
        IDenormalizedHierarchicalPersistentSource<BusinessUnitAncestorLink, BusinessUnitToAncestorChildView, BusinessUnit, Guid>,
        IUnit<BusinessUnit>,
        IMaster<ManagementUnitAndBusinessUnitLink>,
        IMaster<BusinessUnitEmployeeRole>,
        IMaster<BusinessUnitManagerCommissionLink>,
        IMaster<BusinessUnit>,
        IMaster<Project>,
        IDetail<BusinessUnit>,
        IHierarchicalLevelObjectDenormalized,
        ISecurityContext
{
    private readonly ICollection<BusinessUnitEmployeeRole> businessUnitEmployeeRoles = new List<BusinessUnitEmployeeRole>();
    private readonly ICollection<BusinessUnitManagerCommissionLink> managerCommissions = new List<BusinessUnitManagerCommissionLink>();

    private readonly ICollection<BusinessUnit> children = new List<BusinessUnit>();

    private readonly ICollection<Project> projects = new List<Project>();

    private DateTime? firstNewBusinessStatusMonth;
    private int newBusinessStatusLeft;
    private BusinessUnit parent;
    private string projectStartMailList;
    private int rank;
    private DateTime? leastProjectStartDate;
    private DateTime? lastBusinessUnitHasNoLinkedProjectsWarningCheckDate;
    private bool needSendBusinessUnitHasNoLinkedProjectsWarning;
    private BusinessUnitType businessUnitType;
    private BusinessUnitOptions options;

    private BusinessUnit businessUnitForRent;
    private decimal commission;
    private bool isNewBusiness;
    private bool isProduction;
    private Period period;

    private int deepLevel;

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

    [Framework.Restriction.UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndBusinessUnitLink> ManagementUnits { get; } = new List<ManagementUnitAndBusinessUnitLink>();

    public virtual int DeepLevel
    {
        get { return this.deepLevel; }
        protected set { this.deepLevel = value; }
    }

    public virtual DateTime? LastBusinessUnitHasNoLinkedProjectsWarningCheckDate
    {
        get { return this.lastBusinessUnitHasNoLinkedProjectsWarningCheckDate; }
        protected internal set { this.lastBusinessUnitHasNoLinkedProjectsWarningCheckDate = value; }
    }

    public virtual bool NeedSendBusinessUnitHasNoLinkedProjectsWarning
    {
        get { return this.needSendBusinessUnitHasNoLinkedProjectsWarning; }
        protected internal set { this.needSendBusinessUnitHasNoLinkedProjectsWarning = value; }
    }

    public virtual BusinessUnitType BusinessUnitType
    {
        get { return this.businessUnitType; }
        protected internal set { this.businessUnitType = value; }
    }

    public virtual decimal Commission
    {
        get { return this.commission; }
        set { this.commission = value; }
    }

    public virtual BusinessUnitOptions Options
    {
        get { return this.options; }
        set { this.options = value; }
    }

    public virtual bool IsNewBusiness
    {
        get { return this.isNewBusiness; }
        set { this.isNewBusiness = value; }
    }

    public virtual bool IsProduction
    {
        get { return this.isProduction; }
        set { this.isProduction = value; }
    }

    public virtual bool IsSpecialCommission
    {
        get
        {
            return this.options.HasFlag(BusinessUnitOptions.IsSpecialCommission);
        }

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
        get
        {
            return this.options.HasFlag(BusinessUnitOptions.IsResourcePool);
        }

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
        get { return this.businessUnitForRent; }
        set { this.businessUnitForRent = value; }
    }

    public virtual IEnumerable<BusinessUnitEmployeeRole> BusinessUnitEmployeeRoles
    {
        get { return this.businessUnitEmployeeRoles; }
    }

    [UniqueGroup]
    [PropertyValidationMode(PropertyValidationMode.Auto, PropertyValidationMode.Disabled)]
    public virtual IEnumerable<BusinessUnitManagerCommissionLink> ManagerCommissions
    {
        get { return this.managerCommissions; }
    }

    /// <summary>
    /// #IAD-20872
    /// </summary>
    public virtual DateTime? LeastProjectStartDate
    {
        get { return this.leastProjectStartDate; }
        set { this.leastProjectStartDate = value; }
    }

    public virtual Period Period
    {
        get { return this.period; }
        protected internal set { this.period = value; }
    }

    public virtual int Rank
    {
        get { return this.rank; }
        set { this.rank = value; }
    }

    public virtual DateTime? FirstNewBusinessStatusMonth
    {
        get { return this.firstNewBusinessStatusMonth; }
        set { this.firstNewBusinessStatusMonth = value; }
    }

    public virtual int NewBusinessStatusLeft
    {
        get { return this.newBusinessStatusLeft; }
        set { this.newBusinessStatusLeft = value; }
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
        get { return this.projectStartMailList; }
        set { this.projectStartMailList = value; }
    }

    #region ForEventDTO

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit AccountOrDivision
    {
        get
        {
            return
                    this.GetParentByTypeIds(new List<Guid>
                                            {
                                                    BusinessUnitType.AccountTypeId,
                                                    BusinessUnitType.DivisionTypeId
                                            });
        }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit LobOrService
    {
        get
        {
            return
                    this.GetParentByTypeIds(new List<Guid>
                                            {
                                                    BusinessUnitType.LobTypeId,
                                                    BusinessUnitType.ServiceTypeId
                                            });
        }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit Account
    {
        get
        {
            return
                    this.GetParentByTypeIds(new List<Guid>
                                            {
                                                    BusinessUnitType.AccountTypeId,
                                            });
        }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit Program
    {
        get
        {
            return
                    this.GetParentByTypeIds(new List<Guid>
                                            {
                                                    BusinessUnitType.ProgramTypeId,
                                            });
        }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual BusinessUnit Lob
    {
        get
        {
            return
                    this.GetParentByTypeIds(new List<Guid>
                                            {
                                                    BusinessUnitType.LobTypeId,
                                            });
        }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string AccountOrDivisionName
    {
        get { return this.AccountOrDivision.Maybe(z => z.Name); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string LobOrServiceName
    {
        get { return this.LobOrService.Maybe(z => z.Name); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string ProgramName
    {
        get { return this.Program.Maybe(z => z.Name); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual string AccountName
    {
        get { return this.Account.Maybe(z => z.Name); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration)]
    public virtual string LobName
    {
        get { return this.Lob.Maybe(z => z.Name); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid AccountOrDivisionId
    {
        get { return this.AccountOrDivision.Maybe(z => z.Id); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid LobOrServiceId
    {
        get { return this.LobOrService.Maybe(z => z.Id); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid AccountId
    {
        get { return this.Account.Maybe(z => z.Id); }
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client | DTORole.Integration)]
    public virtual Guid LobId
    {
        get { return this.Lob.Maybe(z => z.Id); }
    }

    #endregion

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration | DTORole.Event)]
    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client)]
    public virtual IEnumerable<BusinessUnit> Children
    {
        get { return this.children; }
    }

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual IEnumerable<Project> Projects
    {
        get { return this.projects; }
    }

    ////[CustomSerialization(CustomSerializationMode.Ignore)]
    ////[IgnoreValidation]
    ////public virtual IEnumerable<SeProject> SeProjects
    ////{
    ////    get { return this.Projects.Where(prj => prj is SeProject).Cast<SeProject>(); }
    ////}

    /// <summary>
    /// Supposed to be set from dto only.
    /// </summary>
    [IsMaster]
    public virtual BusinessUnit Parent
    {
        get { return this.parent; }
        protected internal set { this.parent = value; }
    }

    BusinessUnit IUnit<BusinessUnit>.CurrentObject
    {
        get
        {
            return this;
        }
    }

    ICollection<BusinessUnitEmployeeRole> IMaster<BusinessUnitEmployeeRole>.Details
    {
        get
        {
            return (ICollection<BusinessUnitEmployeeRole>)this.BusinessUnitEmployeeRoles;
        }
    }

    ICollection<ManagementUnitAndBusinessUnitLink> IMaster<ManagementUnitAndBusinessUnitLink>.Details
    {
        get
        {
            return (ICollection<ManagementUnitAndBusinessUnitLink>)this.ManagementUnits;
        }
    }

    ICollection<BusinessUnitManagerCommissionLink> IMaster<BusinessUnitManagerCommissionLink>.Details
    {
        get
        {
            return (ICollection<BusinessUnitManagerCommissionLink>)this.ManagerCommissions;
        }
    }

    ICollection<BusinessUnit> IMaster<BusinessUnit>.Details
    {
        get { return (ICollection<BusinessUnit>)this.Children; }
    }

    BusinessUnit IDetail<BusinessUnit>.Master
    {
        get { return this.Parent; }
    }

    ICollection<Project> IMaster<Project>.Details
    {
        get { return (ICollection<Project>)this.Projects; }
    }

    public static bool operator ==(BusinessUnit left, IUnit<BusinessUnit> right)
    {
        return object.Equals(left, right);
    }

    public static bool operator !=(BusinessUnit left, IUnit<BusinessUnit> right)
    {
        return !object.Equals(left, right);
    }

    ////public virtual string GetNodePath()
    ////{
    ////    return UnitExtensions.GetNodePath(this);
    ////}

    ////public virtual IEnumerable<NotificationMessageGenerationInfo> GetNoLinkedProjectsWarningRecipients(BusinessUnit prev)
    ////{
    ////    IEnumerable<IEmployee> recipients = this.BusinessUnitEmployeeRoles
    ////        .Where(r => r.Role == BusinessUnitEmployeeRoleType.Manager)
    ////        .Select(s => s.Employee).ToList();

    ////    yield return new NotificationMessageGenerationInfo(recipients, this, prev);
    ////}
    public virtual void SetDeepLevel(int value) => this.DeepLevel = value;

    public override string ToString()
    {
        return $"{base.ToString()}";
    }

    public virtual DateTime? GetLastNewBusinessStatusMonth()
    {
        if (this.FirstNewBusinessStatusMonth == null)
        {
            return null;
        }

        return this.FirstNewBusinessStatusMonth.Value.AddMonths(this.NewBusinessStatusLeft - 1);
    }

    public virtual BusinessUnit GetParentByTypeIds(IEnumerable<Guid> businessUnitTypesIds)
    {
        return businessUnitTypesIds.Contains(this.BusinessUnitType.Maybe(v => v.Id))
                       ? this
                       : this.Parent.Maybe(z => z.GetParentByTypeIds(businessUnitTypesIds));
    }

    public virtual BusinessUnit GetParentByTypes(IEnumerable<BusinessUnitType> businessUnitTypes)
    {
        return this.GetParentByTypeIds(businessUnitTypes.Select(z => z.Id));
    }

    public virtual string GetProjectStartMailListByHierarahy()
    {
        var result = this.GetAllParents()
                         .Select(z => z.ProjectStartMailList)
                         .Where(z => !string.IsNullOrWhiteSpace(z))
                         .SelectMany(z => z.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                         .Distinct(StringComparer.CurrentCultureIgnoreCase)
                         .ToArray();

        return string.Join(";", result);
    }

    public virtual bool GetNoLinkedProjectsWarningCondition(BusinessUnit prev)
    {
        if (prev != null &&
            prev.NeedSendBusinessUnitHasNoLinkedProjectsWarning &&
            prev.LastBusinessUnitHasNoLinkedProjectsWarningCheckDate.HasValue &&
            prev.LastBusinessUnitHasNoLinkedProjectsWarningCheckDate.Value.Date == DateTime.Today)
        {
            return false;
        }

        if (!this.NeedSendBusinessUnitHasNoLinkedProjectsWarning ||
            !this.LastBusinessUnitHasNoLinkedProjectsWarningCheckDate.HasValue ||
            this.LastBusinessUnitHasNoLinkedProjectsWarningCheckDate.Value.Date != DateTime.Today)
        {
            return false;
        }

        return true;
    }

    public virtual bool CurrentOrParentHasDoNotPrintNameOnLabel()
    {
        return this.GetAllParents().Any(x => x.Options.HasFlag(BusinessUnitOptions.DoNotPrintNameOnLabel));
    }

    public virtual IEnumerable<BusinessUnitEmployeeRoleType> GetCurrentUserRoles(IUserAuthenticationService userAuthenticationService)
    {
        var currentUserName = userAuthenticationService.GetUserName();

        return this.GetAllParents()
                   .SelectMany(bu => bu.BusinessUnitEmployeeRoles)
                   .Where(r => r.Employee.Login == currentUserName && r.Role != BusinessUnitEmployeeRoleType.None)
                   .Select(z => z.Role).Distinct();
    }

    public virtual string GetManagersListCommaSeparatedEng()
    {
        return string.Join(
                           ", ",
                           this.BusinessUnitEmployeeRoles
                               .Where(
                                      z =>
                                              z.Role == BusinessUnitEmployeeRoleType.Manager ||
                                              z.Role == BusinessUnitEmployeeRoleType.ManagerDelegated)
                               .Select(z => z.Employee.NameEng.ToString()));
    }

    public virtual string GetManagersListCommaSeparatedNative()
    {
        return string.Join(
                           ", ",
                           this.BusinessUnitEmployeeRoles
                               .Where(
                                      z =>
                                              z.Role == BusinessUnitEmployeeRoleType.Manager ||
                                              z.Role == BusinessUnitEmployeeRoleType.ManagerDelegated)
                               .Select(z => z.Employee.NameNative.ToString()));
    }
}
