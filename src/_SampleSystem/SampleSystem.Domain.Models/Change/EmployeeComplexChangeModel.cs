using SampleSystem.Domain.Models.Change._Base;

namespace SampleSystem.Domain.Models.Change;

public class EmployeeComplexChangeModel : DomainObjectComplexChangeModel<Employee.Employee>
{
    public string Email { get; set; }
}
