using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public interface ITemplateGenericRepositoryFactory<out TRepository, TDomainObject, in TIdent>
    where TRepository : IGenericRepository<TDomainObject, TIdent>
{
    TRepository Create(ISecurityProvider<TDomainObject> securityProvider);

    TRepository Create(BLLSecurityMode securityMode);

    /// <summary>
    /// Create new Repository without security filters
    /// </summary>
    TRepository Create();
}

public interface ITemplateGenericRepositoryFactory<out TRepository, TDomainObject, in TIdent, TSecurityOperationCode> : ITemplateGenericRepositoryFactory<TRepository, TDomainObject, TIdent>
    where TSecurityOperationCode : struct, Enum
    where TRepository : IGenericRepository<TDomainObject, TIdent>
{
    TRepository Create(TSecurityOperationCode securityOperationCode);

    TRepository Create(SecurityOperation<TSecurityOperationCode> securityOperation);
}
