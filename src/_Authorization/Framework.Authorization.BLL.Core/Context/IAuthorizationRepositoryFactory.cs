using System;
using System.Diagnostics.CodeAnalysis;

using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;

namespace Framework.Authorization.BLL.Core.Context;

[SuppressMessage("SonarQube", "S4023", Justification = "Interface to simplify working with Repository")]
public interface IAuthorizationRepositoryFactory<TDomainObject> : IRepositoryFactory<TDomainObject, Guid, AuthorizationSecurityOperationCode>
        where TDomainObject : PersistentDomainObjectBase
{
}
