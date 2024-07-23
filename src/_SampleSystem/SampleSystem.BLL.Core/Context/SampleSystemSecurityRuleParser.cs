using Framework.DomainDriven;
using Framework.SecuritySystem;

using SampleSystem.Security;

namespace SampleSystem.BLL;

public class SampleSystemSecurityRuleParser : ISecurityRuleParser
{
    public SecurityRule Parse<TDomainObject>(string name)
    {
        switch (name)
        {
            case nameof(SampleSystemSecurityOperation.EmployeeEdit):
                return SampleSystemSecurityOperation.EmployeeEdit;

            default:
                throw new NotImplementedException();
        }
    }
}
