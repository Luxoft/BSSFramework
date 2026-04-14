using Framework.Relations;

namespace SampleSystem.Domain.Employee;

public class EmployeeCellPhone : EmployeeCellPhoneBase
{
    public EmployeeCellPhone(Employee employee)
            : base(employee) =>
        this.Employee.AddDetail(this);

    protected EmployeeCellPhone()
    {
    }
}
