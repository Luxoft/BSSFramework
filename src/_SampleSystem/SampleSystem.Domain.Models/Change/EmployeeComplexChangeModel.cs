namespace SampleSystem.Domain;

public class EmployeeComplexChangeModel : DomainObjectComplexChangeModel<Employee.Employee>
{
    public string Email { get; set; }
}
