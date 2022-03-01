using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL
{
    public partial class WorkflowSampleSystemSecurityService
    {
        public override WorkflowSampleSystemSecurityPath<BusinessUnit> GetBusinessUnitSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<BusinessUnit>.Create(v => v);
        }

        public override WorkflowSampleSystemSecurityPath<Employee> GetEmployeeSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<Employee>.Create(v => v.CoreBusinessUnit);
        }
    }
}
