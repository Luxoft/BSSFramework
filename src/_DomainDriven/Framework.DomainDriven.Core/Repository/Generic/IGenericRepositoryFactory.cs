namespace Framework.DomainDriven.Repository;

public interface IGenericRepositoryFactory<TDomainObject, in TIdent> : ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>,
        TDomainObject>
{
}

public interface IGenericRepositoryFactory<TDomainObject, in TIdent> : IGenericRepositoryFactory<TDomainObject, TIdent>, ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>, TDomainObject>
    where TSecurityOperationCode : struct, Enum
{
}
