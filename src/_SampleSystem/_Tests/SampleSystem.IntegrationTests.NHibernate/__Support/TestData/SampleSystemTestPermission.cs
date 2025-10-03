using Automation.Utils;

using SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.IntegrationTests.__Support.TestData;

public class SampleSystemTestPermission : TestPermissionBuilder
{
    public SampleSystemTestPermission(
        SecurityRole securityRole,
        BusinessUnitIdentityDTO? businessUnit = null,
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

    public IEnumerable<BusinessUnitIdentityDTO> BusinessUnits
    {
        get => this.Restrictions[typeof(BusinessUnit)].Select(v => new BusinessUnitIdentityDTO(v));
        set => this.Restrictions[typeof(BusinessUnit)] = value.Select(v => v.Id).ToList();
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
