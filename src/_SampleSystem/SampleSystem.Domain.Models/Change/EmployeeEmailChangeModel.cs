using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain;

[BLLViewRole(CustomImplementation = true)]
public class EmployeeEmailChangeModel : DomainObjectChangeModel<Employee>
{
    public string Email { get; set; }
}
