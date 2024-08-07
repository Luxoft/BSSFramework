using Framework.Persistent;

namespace SampleSystem.Domain;

public class EmployeeCellPhone : EmployeeCellPhoneBase
{
    public EmployeeCellPhone(Employee employee)
            : base(employee)
    {
        this.Employee.AddDetail(this);
    }

    protected EmployeeCellPhone()
    {
    }
}
