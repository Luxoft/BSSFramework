namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface IClientSecurityRuleInfoSource
{
    IEnumerable<ClientSecurityRuleInfo> GetInfos();
}
