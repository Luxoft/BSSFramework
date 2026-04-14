using Framework.BLL.Domain.ServiceRole;

namespace SampleSystem.Domain;

[BLLViewRole(CustomImplementation = true)]
public class EmployeeEmailChangeModel : DomainObjectChangeModel<Employee.Employee>
{
    public string Email { get; set; }
}
