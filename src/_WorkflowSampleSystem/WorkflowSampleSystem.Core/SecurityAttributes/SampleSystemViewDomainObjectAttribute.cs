using System;
using System.Linq;

using Framework.Security;

namespace WorkflowSampleSystem
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class WorkflowSampleSystemViewDomainObjectAttribute : ViewDomainObjectAttribute
    {
        public WorkflowSampleSystemViewDomainObjectAttribute(Type viewSecurityType)
            : base(viewSecurityType)
        {
        }

        public WorkflowSampleSystemViewDomainObjectAttribute(WorkflowSampleSystemSecurityOperationCode primaryOperation, params WorkflowSampleSystemSecurityOperationCode[] secondaryOperations)
            : base(primaryOperation, secondaryOperations.Cast<Enum>())
        {
        }
    }
}