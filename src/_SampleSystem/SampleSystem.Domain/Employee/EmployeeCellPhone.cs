using Framework.Persistent;
using Framework.Security;

namespace SampleSystem.Domain;

[ViewDomainObject(typeof(Employee))]
[DomainType("9D3EF98C-B857-40EF-A170-DB1285E4CE28")]
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
