using CommonFramework;
using CommonFramework.Auth;

using Framework.BLL.Domain.Fetching;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Enums;
using SampleSystem.Domain.MU;

namespace SampleSystem.Domain.HRDepartment;

[BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
public partial class HRDepartment :
        HRDepartmentBase,
        IMaster<ManagementUnitAndHRDepartmentLink>,
        IMaster<HRDepartmentRoleEmployee>,
        IMaster<HRDepartmentEmployeePosition>,
        IMaster<BusinessUnitHrDepartment>,
        IMaster<HRDepartment>,
        IDetail<HRDepartment>
{
    private readonly ICollection<HRDepartmentRoleEmployee> hrDepartmentRoleEmployees = new List<HRDepartmentRoleEmployee>();
    private readonly ICollection<BusinessUnitHrDepartment> businessUnitHrDepartments = new List<BusinessUnitHrDepartment>();
    private readonly ICollection<HRDepartmentEmployeePosition> employeePositions = new List<HRDepartmentEmployeePosition>();
    private readonly ICollection<HRDepartment> children = new List<HRDepartment>();
    private readonly ICollection<ManagementUnitAndHRDepartmentLink> managementUnits = new List<ManagementUnitAndHRDepartmentLink>();

    private HRDepartment parent;
    private Employee.Employee approvedBy;

    public HRDepartment()
    {
    }

    public HRDepartment(HRDepartment parent)
            : this()
    {
        this.Parent = parent;
        parent?.children.Add(this);
    }

    /// <summary>
    /// For unit tests
    /// </summary>
    public HRDepartment(ManagementUnitAndHRDepartmentLink managementUnitAndHRDepartmentLink)
            : this()
    {
        if (null != managementUnitAndHRDepartmentLink)
        {
            this.managementUnits.Add(managementUnitAndHRDepartmentLink);
        }
    }

    [UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndHRDepartmentLink> ManagementUnits => this.managementUnits;

    [Required]
    public override CompanyLegalEntity CompanyLegalEntity
    {
        get => base.CompanyLegalEntity;
        set => base.CompanyLegalEntity = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    [Required]
    public override Location Location
    {
        get => base.Location;
        set => base.Location = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client | DTORole.Report)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
    public virtual IEnumerable<HRDepartment> Children => this.children;

    public virtual HRDepartment Parent
    {
        get => this.parent;
        set => this.parent = value;
    }

    public virtual Employee.Employee ApprovedBy
    {
        get => this.approvedBy;
        set => this.approvedBy = value;
    }

    [CustomSerialization(CustomSerializationMode.Normal)]
    public override bool Active
    {
        get => base.Active;
        set => base.Active = value;
    }

    [UniqueGroup]
    public virtual IEnumerable<HRDepartmentRoleEmployee> HrDepartmentRoleEmployees => this.hrDepartmentRoleEmployees;

    [Obsolete("Now HRDepartment linked to ManagementBusinessUnit")]
    [UniqueGroup]
    public virtual IEnumerable<BusinessUnitHrDepartment> BusinessUnitHrDepartments => this.businessUnitHrDepartments;

    [UniqueGroup]
    public virtual IEnumerable<HRDepartmentEmployeePosition> EmployeePositions => this.employeePositions;

    [FetchPath("Location")]
    public virtual string LocationName
    {
        get
        {
            if (this.Location == null)
            {
                return string.Empty;
            }

            return this.Location.Name;
        }
    }

    public virtual string CompanyLegalEntityName
    {
        get
        {
            if (this.CompanyLegalEntity == null)
            {
                return string.Empty;
            }

            return this.CompanyLegalEntity.Name;
        }
    }

    ICollection<HRDepartmentRoleEmployee> IMaster<HRDepartmentRoleEmployee>.Details => (ICollection<HRDepartmentRoleEmployee>)this.HrDepartmentRoleEmployees;

    ICollection<HRDepartmentEmployeePosition> IMaster<HRDepartmentEmployeePosition>.Details => (ICollection<HRDepartmentEmployeePosition>)this.EmployeePositions;

    ICollection<ManagementUnitAndHRDepartmentLink> IMaster<ManagementUnitAndHRDepartmentLink>.Details => (ICollection<ManagementUnitAndHRDepartmentLink>)this.ManagementUnits;

    ICollection<BusinessUnitHrDepartment> IMaster<BusinessUnitHrDepartment>.Details => (ICollection<BusinessUnitHrDepartment>)this.BusinessUnitHrDepartments;

    ICollection<HRDepartment> IMaster<HRDepartment>.Details => (ICollection<HRDepartment>)this.Children;

    HRDepartment IDetail<HRDepartment>.Master => this.Parent;

    public virtual IEnumerable<HRDepartmentEmployeeRoleType> GetCurrentUserRoles(ICurrentUser currentUser)
    {
        var currentUserName = currentUser.Name.ToLower();

        return this.hrDepartmentRoleEmployees
                   .Where(r =>
                                  string.Equals(r.Employee.Login.Maybe(z => z.ToLower()), currentUserName)
                                  && r.HRDepartmentEmployeeRoleType != HRDepartmentEmployeeRoleType.None)
                   .Select(z => z.HRDepartmentEmployeeRoleType)
                   .Distinct();
    }

    public virtual IEnumerable<Employee.Employee> GetHRInspectors() =>
        this.hrDepartmentRoleEmployees
            .Where(z => z.HRDepartmentEmployeeRoleType == HRDepartmentEmployeeRoleType.Inspector)
            .Select(z => z.Employee);

    public virtual bool CurrentUserHasInspectorRoles(ICurrentUser currentUser)
    {
        var inspectorRoleType = this.GetCurrentUserRoles(currentUser).FirstOrDefault(z => z == HRDepartmentEmployeeRoleType.Inspector);
        return HRDepartmentEmployeeRoleType.None != inspectorRoleType;
    }
}
