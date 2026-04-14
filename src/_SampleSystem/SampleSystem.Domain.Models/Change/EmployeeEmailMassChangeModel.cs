using SampleSystem.Domain.Models.Change._Base;

namespace SampleSystem.Domain.Models.Change;

public class EmployeeEmailMassChangeModel : DomainObjectMassChangeModel<Employee.Employee>
{
    public string Email { get; set; }
}
