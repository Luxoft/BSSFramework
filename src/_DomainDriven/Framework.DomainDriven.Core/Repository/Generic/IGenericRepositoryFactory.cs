namespace Framework.DomainDriven.Repository;

public interface IGenericRepositoryFactory<TDomainObject, in TIdent> : ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>,
        TDomainObject>
{
}

public interface IGenericRepositoryFactory<TDomainObject, in TIdent, TSecurityOperationCode> : IGenericRepositoryFactory<TDomainObject, TIdent>, ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>, TDomainObject, TSecurityOperationCode>
    where TSecurityOperationCode : struct, Enum
{
}
