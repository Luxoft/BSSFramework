using Anch.SecuritySystem;

namespace Framework.BLL.Domain.Attributes;

/// <summary>
/// Attribute for editing an object (or its property)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class EditDomainObjectAttribute : DomainObjectAccessAttribute
{
    public EditDomainObjectAttribute(Type securityRuleType, string name)
        : base(securityRuleType, name)
    {
    }

    public EditDomainObjectAttribute(Type domainType, bool isEdit = true)
        : base(domainType, isEdit)
    {
    }

    public EditDomainObjectAttribute(SecurityRule securityRule)
        : base(securityRule)
    {
    }
}
