﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.DomainDriven;

public interface IAsyncDal<TDomainObject, in TIdent>
{
    IQueryable<TDomainObject> GetQueryable();

    TDomainObject Load(object id);

    Task<TDomainObject> LoadAsync(object id, CancellationToken cancellationToken = default);

    Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);

    Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default);

    Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);
}
