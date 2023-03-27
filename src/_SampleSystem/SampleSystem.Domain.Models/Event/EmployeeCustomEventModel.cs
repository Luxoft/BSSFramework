using JetBrains.Annotations;

namespace SampleSystem.Domain;

public class EmployeeCustomEventModel : DomainObjectBase
{
    public EmployeeCustomEventModel()
    {
    }

    public EmployeeCustomEventModel([NotNull] Employee employee)
    {
        if (employee == null) { throw new ArgumentNullException(nameof(employee)); }

        this.Id = employee.Id;
        this.Login = employee.Login;
    }

    public Guid Id { get; set; }

    public string Login { get; set; }
}
