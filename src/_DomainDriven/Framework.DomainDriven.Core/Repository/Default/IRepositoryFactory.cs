namespace Framework.DomainDriven.Repository;

public interface IRepositoryFactory<TDomainObject> : ITemplateGenericRepositoryFactory<IRepository<TDomainObject>, TDomainObject>
{
}

public interface IRepositoryFactory<TDomainObject> : IRepositoryFactory<TDomainObject>,
                                                                                    ITemplateGenericRepositoryFactory<IRepository<TDomainObject>,
                                                                                    TDomainObject>
    where TSecurityOperationCode : struct, Enum
{
}
