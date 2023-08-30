namespace SampleSystem.Domain;

public class EmployeeComplexChangeModel : DomainObjectComplexChangeModel<Employee>
{
    public string Email { get; set; }
}
