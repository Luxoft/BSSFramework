using Framework.Relations;

namespace SampleSystem.Domain.Employee;

public class EmployeePersonalCellPhone : EmployeeCellPhoneBase
{
    public EmployeePersonalCellPhone(Employee employee)
            : base(employee) =>
        this.Employee.AddDetail(this);

    protected EmployeePersonalCellPhone()
    {
    }
}
