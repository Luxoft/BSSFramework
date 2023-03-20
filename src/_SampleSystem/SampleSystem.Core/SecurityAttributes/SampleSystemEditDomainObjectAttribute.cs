using System;
using Framework.Security;

namespace SampleSystem;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class SampleSystemEditDomainObjectAttribute : EditDomainObjectAttribute
{
    public SampleSystemEditDomainObjectAttribute(SampleSystemSecurityOperationCode securityOperation)
            : base(securityOperation)
    {
    }

    public SampleSystemEditDomainObjectAttribute(Type editSecurityType)
            : base(editSecurityType)
    {
    }
}
