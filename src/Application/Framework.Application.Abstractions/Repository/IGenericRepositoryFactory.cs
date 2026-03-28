namespace Framework.Application.Repository;

public interface IGenericRepositoryFactory<TDomainObject, in TIdent> : ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>,
        TDomainObject>
{
}
