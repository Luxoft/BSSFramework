using System;

using SampleSystem.BLL;
using Framework.CustomReports.Domain;
using Framework.SecuritySystem;

namespace SampleSystem.WebApiCore.CustomReports
{
    public class SecurityOperationCodeProvider : ISecurityOperationCodeProvider<SampleSystemSecurityOperationCode>
    {
        public SampleSystemSecurityOperationCode GetByDomain(Type domainType, BLLSecurityMode mode)
        {
            return SampleSystemSecurityOperation.GetCodeByMode(domainType, mode);
        }
    }
}
