using System;
using Framework.Security;

namespace Framework.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class AuthorizationViewDomainObjectAttribute : ViewDomainObjectAttribute
    {
        public AuthorizationViewDomainObjectAttribute(AuthorizationSecurityOperationCode securityOperation)
            : base(securityOperation)
        {
        }

        public AuthorizationViewDomainObjectAttribute(Type viewSecurityType)
            : base(viewSecurityType)
        {
        }
    }
}