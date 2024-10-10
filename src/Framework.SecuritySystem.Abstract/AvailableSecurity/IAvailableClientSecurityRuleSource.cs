namespace Framework.SecuritySystem.AvailableSecurity;

public interface IAvailableClientSecurityRuleSource
{
    Task<List<ClientSecurityRuleHeader>> GetAvailableSecurityRules(CancellationToken cancellationToken = default);
}
