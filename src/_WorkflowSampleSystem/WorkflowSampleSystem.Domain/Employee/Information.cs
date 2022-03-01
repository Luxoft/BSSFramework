using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole]
    [BLLEventRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    public class Information : BaseDirectory
    {
        private string email;

        [MaxLength(50)]
        public virtual string Email
        {
            get { return this.email.TrimNull(); }
            set { this.email = value.TrimNull(); }
        }
    }
}
