using Framework.Persistent;
using Framework.Security;

namespace SampleSystem.Domain;

[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView))]
[EditDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.EmployeePersonalCellPhoneEdit))]
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
