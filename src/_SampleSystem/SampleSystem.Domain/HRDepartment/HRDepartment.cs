using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace SampleSystem.Domain;

[DomainType("0BE31997-C4CD-449E-9394-A311016CB715")]
[BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.HRDepartmentView, SampleSystemSecurityOperationCode.EmployeeEdit)]
[SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.HRDepartmentEdit)]
public partial class HRDepartment :
        HRDepartmentBase,
        IDefaultHierarchicalPersistentDomainObjectBase<HRDepartment>,
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
    private Employee approvedBy;

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

    [Framework.Restriction.UniqueGroup]
    public virtual IEnumerable<ManagementUnitAndHRDepartmentLink> ManagementUnits
    {
        get { return this.managementUnits; }
    }

    [Framework.Restriction.Required]
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.CompanyLegalEntityView)]
    [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.CompanyLegalEntityEdit)]
    public override CompanyLegalEntity CompanyLegalEntity
    {
        get { return base.CompanyLegalEntity; }
        set { base.CompanyLegalEntity = value; }
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    [Framework.Restriction.Required]
    public override Location Location
    {
        get { return base.Location; }
        set { base.Location = value; }
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client | DTORole.Report)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
    public virtual IEnumerable<HRDepartment> Children
    {
        get { return this.children; }
    }

    public virtual HRDepartment Parent
    {
        get { return this.parent; }
        set { this.parent = value; }
    }

    public virtual Employee ApprovedBy
    {
        get { return this.approvedBy; }
        set { this.approvedBy = value; }
    }

    [CustomSerialization(CustomSerializationMode.Normal)]
    public override bool Active
    {
        get { return base.Active; }
        set { base.Active = value; }
    }

    [Framework.Restriction.UniqueGroup]
    public virtual IEnumerable<HRDepartmentRoleEmployee> HrDepartmentRoleEmployees
    {
        get { return this.hrDepartmentRoleEmployees; }
    }

    [Obsolete("Now HRDepartment linked to ManagementBusinessUnit")]
    [Framework.Restriction.UniqueGroup]
    public virtual IEnumerable<BusinessUnitHrDepartment> BusinessUnitHrDepartments
    {
        get { return this.businessUnitHrDepartments; }
    }

    [Framework.Restriction.UniqueGroup]
    public virtual IEnumerable<HRDepartmentEmployeePosition> EmployeePositions
    {
        get { return this.employeePositions; }
    }

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

    ICollection<HRDepartmentRoleEmployee> IMaster<HRDepartmentRoleEmployee>.Details
    {
        get
        {
            return (ICollection<HRDepartmentRoleEmployee>)this.HrDepartmentRoleEmployees;
        }
    }

    ICollection<HRDepartmentEmployeePosition> IMaster<HRDepartmentEmployeePosition>.Details
    {
        get
        {
            return (ICollection<HRDepartmentEmployeePosition>)this.EmployeePositions;
        }
    }

    ICollection<ManagementUnitAndHRDepartmentLink> IMaster<ManagementUnitAndHRDepartmentLink>.Details
    {
        get
        {
            return (ICollection<ManagementUnitAndHRDepartmentLink>)this.ManagementUnits;
        }
    }

    ICollection<BusinessUnitHrDepartment> IMaster<BusinessUnitHrDepartment>.Details
    {
        get { return (ICollection<BusinessUnitHrDepartment>)this.BusinessUnitHrDepartments; }
    }

    ICollection<HRDepartment> IMaster<HRDepartment>.Details
    {
        get { return (ICollection<HRDepartment>)this.Children; }
    }

    HRDepartment IDetail<HRDepartment>.Master
    {
        get { return this.Parent; }
    }

    public virtual IEnumerable<HRDepartmentEmployeeRoleType> GetCurrentUserRoles(IUserAuthenticationService userAuthenticationService)
    {
        var currentUserName = userAuthenticationService.GetUserName().ToLower();
        return this.hrDepartmentRoleEmployees
                   .Where(r =>
                                  string.Equals(r.Employee.Login.Maybe(z => z.ToLower()), currentUserName)
                                  && r.HRDepartmentEmployeeRoleType != HRDepartmentEmployeeRoleType.None)
                   .Select(z => z.HRDepartmentEmployeeRoleType)
                   .Distinct();
    }

    public virtual IEnumerable<Employee> GetHRInspectors()
    {
        return
                this.hrDepartmentRoleEmployees
                    .Where(z => z.HRDepartmentEmployeeRoleType == HRDepartmentEmployeeRoleType.Inspector)
                    .Select(z => z.Employee);
    }

    public virtual bool CurrentUserHasInspectorRoles(IUserAuthenticationService userAuthenticationService)
    {
        var inspectorRoleType = this.GetCurrentUserRoles(userAuthenticationService).FirstOrDefault(z => z == HRDepartmentEmployeeRoleType.Inspector);
        return HRDepartmentEmployeeRoleType.None != inspectorRoleType;
    }
}
