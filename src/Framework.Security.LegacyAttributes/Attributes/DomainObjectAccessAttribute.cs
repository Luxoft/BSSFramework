using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class DomainObjectAccessAttribute : Attribute
{
    public DomainObjectAccessAttribute(Type securityOperationType, string name)
        : this(securityOperationType.Maybe(v => v.GetSecurityOperation(name)))
    {
    }

    protected DomainObjectAccessAttribute(SecurityRule securityRule)
    {
        this.SecurityRule = securityRule;
    }


    public SecurityRule SecurityRule { get; private set; }


    public bool HasContext => this.SecurityRule is SecurityRule;
}
