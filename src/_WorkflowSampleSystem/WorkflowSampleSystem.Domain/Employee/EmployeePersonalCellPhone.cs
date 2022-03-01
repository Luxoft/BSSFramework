using Framework.Persistent;

namespace WorkflowSampleSystem.Domain
{
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeePersonalCellPhoneView)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeePersonalCellPhoneEdit)]
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
