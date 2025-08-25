using SecuritySystem;
using SecuritySystem.Providers;

namespace Framework.DomainDriven.Repository;

public interface ITemplateGenericRepositoryFactory<out TRepository, TDomainObject>
{
    /// <summary>
    /// Create new Repository without security filters
    /// </summary>
    TRepository Create();

    TRepository Create(SecurityRule securityRule);

    TRepository Create(ISecurityProvider<TDomainObject> securityProvider);
}
