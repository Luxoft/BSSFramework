using Framework.BLL.Domain.ServiceRole;

using SampleSystem.Domain.Models.Change._Base;

namespace SampleSystem.Domain.Models.Change;

[BLLViewRole(CustomImplementation = true)]
public class EmployeeEmailChangeModel : DomainObjectChangeModel<Employee.Employee>
{
    public string Email { get; set; }
}
