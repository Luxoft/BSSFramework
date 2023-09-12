using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public interface ITemplateGenericRepositoryFactory<out TRepository, TDomainObject>
{
    TRepository Create(ISecurityProvider<TDomainObject> securityProvider);

    TRepository Create(BLLSecurityMode securityMode);

    /// <summary>
    /// Create new Repository without security filters
    /// </summary>
    TRepository Create();
}

public interface ITemplateGenericRepositoryFactory<out TRepository, TDomainObject> : ITemplateGenericRepositoryFactory<TRepository, TDomainObject>
    where TSecurityOperationCode : struct, Enum
{
    TRepository Create(SecurityOperation securityOperation);

    TRepository Create(SecurityOperation securityOperation);
}
