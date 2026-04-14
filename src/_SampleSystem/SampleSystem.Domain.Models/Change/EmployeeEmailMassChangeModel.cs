namespace SampleSystem.Domain;

public class EmployeeEmailMassChangeModel : DomainObjectMassChangeModel<Employee.Employee>
{
    public string Email { get; set; }
}
