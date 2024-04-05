using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public interface ITemplateGenericRepositoryFactory<out TRepository, TDomainObject>
{
    /// <summary>
    /// Create new Repository without security filters
    /// </summary>
    TRepository Create();

    TRepository Create(SecurityRule securityMode);

    TRepository Create(SecurityOperation securityOperation);

    TRepository Create(ISecurityProvider<TDomainObject> securityProvider);
}
