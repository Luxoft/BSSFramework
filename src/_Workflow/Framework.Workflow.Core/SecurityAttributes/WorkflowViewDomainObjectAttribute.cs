using System;

using Framework.Security;

namespace Framework.Workflow
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Property)]
    public class WorkflowViewDomainObjectAttribute : ViewDomainObjectAttribute
    {
        public WorkflowViewDomainObjectAttribute()
            : this(WorkflowSecurityOperationCode.WorkflowView)
        {
        }

        public WorkflowViewDomainObjectAttribute(WorkflowSecurityOperationCode securityOperation)
            : base(securityOperation)
        {
        }
    }
}