namespace Framework.DomainDriven.Repository;

public interface IDefaultRepositoryFactory<TDomainObject> : ITemplateGenericRepositoryFactory<IRepository<TDomainObject>, TDomainObject>
{
}

public interface IDefaultRepositoryFactory<TDomainObject, TSecurityOperationCode> : IDefaultRepositoryFactory<TDomainObject>,
                                                                                    ITemplateGenericRepositoryFactory<IRepository<TDomainObject>,
                                                                                    TDomainObject, TSecurityOperationCode>
    where TSecurityOperationCode : struct, Enum
{
}
