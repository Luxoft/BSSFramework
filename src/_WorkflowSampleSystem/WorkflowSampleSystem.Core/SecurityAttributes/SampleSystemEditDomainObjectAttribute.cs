using System;
using Framework.Security;

namespace WorkflowSampleSystem
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class WorkflowSampleSystemEditDomainObjectAttribute : EditDomainObjectAttribute
    {
        public WorkflowSampleSystemEditDomainObjectAttribute(WorkflowSampleSystemSecurityOperationCode securityOperation)
            : base(securityOperation)
        {
        }

        public WorkflowSampleSystemEditDomainObjectAttribute(Type editSecurityType)
            : base(editSecurityType)
        {
        }
    }
}
