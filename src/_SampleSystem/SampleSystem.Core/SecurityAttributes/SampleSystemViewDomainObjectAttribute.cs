using System;
using System.Linq;

using Framework.Security;

namespace SampleSystem;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class SampleSystemViewDomainObjectAttribute : ViewDomainObjectAttribute
{
    public SampleSystemViewDomainObjectAttribute(Type viewSecurityType)
            : base(viewSecurityType)
    {
    }

    public SampleSystemViewDomainObjectAttribute(SampleSystemSecurityOperationCode primaryOperation, params SampleSystemSecurityOperationCode[] secondaryOperations)
            : base(primaryOperation, secondaryOperations.Cast<Enum>())
    {
    }
}
