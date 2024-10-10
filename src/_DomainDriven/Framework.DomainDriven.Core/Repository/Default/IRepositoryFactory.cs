namespace Framework.DomainDriven.Repository;

public interface IRepositoryFactory<TDomainObject> : ITemplateGenericRepositoryFactory<IRepository<TDomainObject>, TDomainObject>;
