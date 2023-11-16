using Automation.Utils;

using Framework.Authorization.SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.IntegrationTests.__Support.TestData;

public class SampleSystemTestPermission : TestPermission
{
    public SampleSystemTestPermission(
        string securityRoleName,
        BusinessUnitIdentityDTO? businessUnit,
        ManagementUnitIdentityDTO? managementUnit = null,
        LocationIdentityDTO? location = null,
        EmployeeIdentityDTO? employee = null)
        : base(securityRoleName)
    {
        this.BusinessUnit = businessUnit;
        this.ManagementUnit = managementUnit;
        this.Location = location;
        this.Employee = employee;
    }

    public SampleSystemTestPermission(
        SecurityRole securityRole,
        BusinessUnitIdentityDTO? businessUnit,
        ManagementUnitIdentityDTO? managementUnit = null,
        LocationIdentityDTO? location = null,
        EmployeeIdentityDTO? employee = null)
        : base(securityRole)
    {
        this.BusinessUnit = businessUnit;
        this.ManagementUnit = managementUnit;
        this.Location = location;
        this.Employee = employee;
    }

    public ManagementUnitIdentityDTO? ManagementUnit
    {
        get => this.GetSingleIdentity(typeof(ManagementUnit), v => new ManagementUnitIdentityDTO(v));
        set => this.SetSingleIdentity(typeof(ManagementUnit), v => v.Id, value);
    }

    public BusinessUnitIdentityDTO? BusinessUnit
    {
        get => this.GetSingleIdentity(typeof(BusinessUnit), v => new BusinessUnitIdentityDTO(v));
        set => this.SetSingleIdentity(typeof(BusinessUnit), v => v.Id, value);
    }

    public LocationIdentityDTO? Location
    {
        get => this.GetSingleIdentity(typeof(Location), v => new LocationIdentityDTO(v));
        set => this.SetSingleIdentity(typeof(Location), v => v.Id, value);
    }

    public EmployeeIdentityDTO? Employee
    {
        get => this.GetSingleIdentity(typeof(Employee), v => new EmployeeIdentityDTO(v));
        set => this.SetSingleIdentity(typeof(Employee), v => v.Id, value);
    }
}
