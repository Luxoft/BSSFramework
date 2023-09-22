using Framework.Persistent;

namespace SampleSystem.Domain;

public class EmployeePersonalCellPhone : EmployeeCellPhoneBase
{
    public EmployeePersonalCellPhone(Employee employee)
            : base(employee)
    {
        this.Employee.AddDetail(this);
    }

    protected EmployeePersonalCellPhone()
    {
    }
}
