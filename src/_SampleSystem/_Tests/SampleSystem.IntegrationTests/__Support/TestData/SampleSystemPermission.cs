using Automation.ServiceEnvironment;
using Automation.Utils;

using SampleSystem.Generated.DTO;


namespace SampleSystem.IntegrationTests.__Support.TestData;

public class SampleSystemPermission : IPermissionDefinition
{
    public SampleSystemPermission()
    {
    }

    public SampleSystemPermission(TestBusinessRole role)
    {
        this.Role = role;
    }

    public SampleSystemPermission(
            TestBusinessRole role,
            BusinessUnitIdentityDTO? businessUnit,
            ManagementUnitIdentityDTO? managementUnit = null,
            LocationIdentityDTO? location = null,
            EmployeeIdentityDTO? employee = null)
    {
        this.Role = role;
        this.BusinessUnit = businessUnit;
        this.ManagementUnit = managementUnit;
        this.Location = location;
        this.Employee = employee;
    }

    public TestBusinessRole Role { get; set; }

    public ManagementUnitIdentityDTO? ManagementUnit { get; set; }

    public BusinessUnitIdentityDTO? BusinessUnit { get; set; }

    public LocationIdentityDTO? Location { get; set; }

    public EmployeeIdentityDTO? Employee { get; set; }

    public IEnumerable<Tuple<string, Guid>> GetEntities()
    {
        if (this.BusinessUnit != null)
        {
            yield return Tuple.Create(nameof(SampleSystem.Domain.BusinessUnit), ((BusinessUnitIdentityDTO)this.BusinessUnit).Id);
        }

        if (this.ManagementUnit != null)
        {
            yield return Tuple.Create(nameof(SampleSystem.Domain.ManagementUnit), ((ManagementUnitIdentityDTO)this.ManagementUnit).Id);
        }

        if (this.Location != null)
        {
            yield return Tuple.Create(nameof(SampleSystem.Domain.Location), ((LocationIdentityDTO)this.Location).Id);
        }

        if (this.Employee != null)
        {
            yield return Tuple.Create(nameof(SampleSystem.Domain.Employee), ((EmployeeIdentityDTO)this.Employee).Id);
        }
    }

    public string GetRoleName()
    {
        return this.Role.GetRoleName();
    }
}
