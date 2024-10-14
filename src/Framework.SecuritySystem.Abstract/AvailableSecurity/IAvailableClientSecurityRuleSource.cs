namespace Framework.SecuritySystem.AvailableSecurity;

public interface IAvailableClientSecurityRuleSource
{
    Task<List<DomainSecurityRule.ClientSecurityRule>> GetAvailableSecurityRules(CancellationToken cancellationToken = default);
}
