namespace Framework.DomainDriven.Repository;

public interface IRepositoryFactory<TDomainObject> : ITemplateGenericRepositoryFactory<IRepository<TDomainObject>, TDomainObject>
{
}

public interface IRepositoryFactory<TDomainObject, TSecurityOperationCode> : IRepositoryFactory<TDomainObject>,
                                                                                    ITemplateGenericRepositoryFactory<IRepository<TDomainObject>,
                                                                                    TDomainObject, TSecurityOperationCode>
    where TSecurityOperationCode : struct, Enum
{
}
