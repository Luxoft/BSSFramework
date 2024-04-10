using Framework.SecuritySystem;

namespace Framework.DomainDriven;

public interface ISecurityRuleParser
{
    SecurityRule Parse<TDomainObject>(string name);
}
