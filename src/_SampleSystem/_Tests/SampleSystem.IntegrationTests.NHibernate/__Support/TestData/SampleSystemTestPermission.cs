using CommonFramework;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

using SecuritySystem;
using SecuritySystem.Testing;

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
        get => this.GetSingle<ManagementUnit, Guid>().Maybe(v => new ManagementUnitIdentityDTO(v.Id));
        set => this.SetSingle<ManagementUnit, Guid>(value.MaybeNullable(v => TypedSecurityIdentity.Create(v.Id)));
    }

    public BusinessUnitIdentityDTO? BusinessUnit
    {
        get => this.GetSingle<BusinessUnit, Guid>().Maybe(v => new BusinessUnitIdentityDTO(v.Id));
        set => this.SetSingle<BusinessUnit, Guid>(value.MaybeNullable(v => TypedSecurityIdentity.Create(v.Id)));
    }

    public IEnumerable<BusinessUnitIdentityDTO> BusinessUnits
    {
        get => this.Restrictions[typeof(BusinessUnit)].Cast<Guid>().Select(v => new BusinessUnitIdentityDTO(v));
        set => this.Restrictions[typeof(BusinessUnit)] = value.Select(v => v.Id).ToArray();
    }

    public LocationIdentityDTO? Location
    {
        get => this.GetSingle<Location, Guid>().Maybe(v => new LocationIdentityDTO(v.Id));
        set => this.SetSingle<Location, Guid>(value.MaybeNullable(v => TypedSecurityIdentity.Create(v.Id)));
    }

    public EmployeeIdentityDTO? Employee
    {
        get => this.GetSingle<Employee, Guid>().Maybe(v => new EmployeeIdentityDTO(v.Id));
        set => this.SetSingle<Employee, Guid>(value.MaybeNullable(v => TypedSecurityIdentity.Create(v.Id)));
    }
}
