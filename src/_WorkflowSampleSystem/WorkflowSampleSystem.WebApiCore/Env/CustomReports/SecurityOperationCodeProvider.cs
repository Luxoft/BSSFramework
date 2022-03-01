using System;

using WorkflowSampleSystem.BLL;
using Framework.CustomReports.Domain;
using Framework.SecuritySystem;

namespace WorkflowSampleSystem.WebApiCore.CustomReports
{
    public class SecurityOperationCodeProvider : ISecurityOperationCodeProvider<WorkflowSampleSystemSecurityOperationCode>
    {
        public WorkflowSampleSystemSecurityOperationCode GetByDomain(Type domainType, BLLSecurityMode mode)
        {
            return WorkflowSampleSystemSecurityOperation.GetCodeByMode(domainType, mode);
        }
    }
}
