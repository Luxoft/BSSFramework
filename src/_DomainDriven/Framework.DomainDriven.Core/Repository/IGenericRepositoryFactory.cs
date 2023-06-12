using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

//public interface IGenericRepositoryFactory<TDomainObject, in TIdent> : ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>, TDomainObject, Guid>
//{
//}

//public interface IGenericRepositoryFactory<TDomainObject, in TIdent, TSecurityOperationCode> : IGenericRepositoryFactory<TDomainObject, TIdent>
//    where TSecurityOperationCode : struct, Enum
//{
//    IGenericRepository<TDomainObject, TIdent> Create(TSecurityOperationCode securityOperationCode);

//    IGenericRepository<TDomainObject, TIdent> Create(SecurityOperation<TSecurityOperationCode> securityOperation);
//}
