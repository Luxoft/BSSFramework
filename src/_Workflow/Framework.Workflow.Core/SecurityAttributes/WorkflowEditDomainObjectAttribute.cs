using System;

using Framework.Security;

namespace Framework.Workflow
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Property)]
    public class WorkflowEditDomainObjectAttribute : EditDomainObjectAttribute
    {
        public WorkflowEditDomainObjectAttribute()
            : this(WorkflowSecurityOperationCode.WorkflowEdit)
        {
        }

        public WorkflowEditDomainObjectAttribute(WorkflowSecurityOperationCode securityOperation)
            : base(securityOperation)
        {
        }
    }
}