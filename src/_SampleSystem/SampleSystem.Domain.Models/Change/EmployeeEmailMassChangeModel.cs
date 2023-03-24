namespace SampleSystem.Domain;

public class EmployeeEmailMassChangeModel : DomainObjectMassChangeModel<Employee>
{
    public string Email { get; set; }
}
