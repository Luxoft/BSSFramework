using Framework.Security;

namespace Framework.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class AuthorizationEditDomainObjectAttribute : EditDomainObjectAttribute
{
    public AuthorizationEditDomainObjectAttribute(AuthorizationSecurityOperationCode securityOperation)
            : base(securityOperation)
    {
    }

    public AuthorizationEditDomainObjectAttribute(Type editSecurityType)
            : base(editSecurityType)
    {
    }
}
