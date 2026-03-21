namespace Framework.Application.Repository;

public interface IRepositoryFactory<TDomainObject> : ITemplateGenericRepositoryFactory<IRepository<TDomainObject>, TDomainObject>;
