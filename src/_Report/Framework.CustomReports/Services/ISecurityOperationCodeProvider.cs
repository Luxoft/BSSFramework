using System;

using Framework.SecuritySystem;

namespace Framework.CustomReports.Domain
{
    public interface ISecurityOperationCodeProvider<out TSecurityOperationCode>
    {
        TSecurityOperationCode GetByDomain(Type domainType, BLLSecurityMode mode);
    }
}
