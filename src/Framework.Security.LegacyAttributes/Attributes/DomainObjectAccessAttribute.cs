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

    public DomainObjectAccessAttribute(Type domainType, bool isEdit)
        : this((isEdit ? SecurityRule.Edit : SecurityRule.View).ToDomain(domainType))
    {
    }

    protected DomainObjectAccessAttribute(SecurityRule securityRule)
    {
        this.SecurityRule = securityRule;
    }


    public SecurityRule SecurityRule { get; }
}
