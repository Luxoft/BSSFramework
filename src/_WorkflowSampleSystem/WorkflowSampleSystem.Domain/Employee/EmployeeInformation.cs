using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    public class EmployeeInformation : Information
    {
        private string personalEmail;

        [MaxLength(50)]
        public virtual string PersonalEmail
        {
            get { return this.personalEmail.TrimNull(); }
            set { this.personalEmail = value.TrimNull(); }
        }
    }
}
