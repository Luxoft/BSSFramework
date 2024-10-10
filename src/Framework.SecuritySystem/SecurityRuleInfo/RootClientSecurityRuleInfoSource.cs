using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class RootClientSecurityRuleInfoSource(
    [FromKeyedServices(RootClientSecurityRuleInfoSource.ElementKey)] IEnumerable<IClientSecurityRuleInfoSource> elements)
    : IClientSecurityRuleInfoSource
{
    public const string ElementKey = "Element";

    private readonly ClientSecurityRuleInfo[] infos = elements.SelectMany(el => el.GetInfos()).Distinct().ToArray();

    public IEnumerable<ClientSecurityRuleInfo> GetInfos() => this.infos;
}
