using System;

using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public interface IRepositoryFactory<TDomainObject, in TIdent, TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum
{
    IRepository<TDomainObject, TIdent> Create(ISecurityProvider<TDomainObject> securityProvider);

    IRepository<TDomainObject, TIdent> Create(TSecurityOperationCode securityOperationCode);

    IRepository<TDomainObject, TIdent> Create(SecurityOperation<TSecurityOperationCode> securityOperation);

    IRepository<TDomainObject, TIdent> Create(BLLSecurityMode securityMode);

    IRepository<TDomainObject, TIdent> Create();
}
