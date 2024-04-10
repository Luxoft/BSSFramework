using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class DomainObjectAccessAttribute : Attribute
{
    public DomainObjectAccessAttribute(Type securityRuleType, string name)
        : this(securityRuleType.Maybe(v => v.GetSecurityRule(name)))
    {
    }

    protected DomainObjectAccessAttribute(SecurityRule securityRule)
    {
        this.SecurityRule = securityRule;
    }


    public SecurityRule SecurityRule { get; private set; }


    public bool HasContext => this.SecurityRule is SecurityRule;
}
