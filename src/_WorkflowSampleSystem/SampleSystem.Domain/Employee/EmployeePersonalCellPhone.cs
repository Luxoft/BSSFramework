using Framework.Persistent;

namespace SampleSystem.Domain
{
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeePersonalCellPhoneView)]
    [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.EmployeePersonalCellPhoneEdit)]
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
}
