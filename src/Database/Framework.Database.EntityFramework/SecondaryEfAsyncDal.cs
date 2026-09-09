using Anch.IdentitySource;

using Framework.Database.EntityFramework.Sessions;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework;

public class SecondaryEfAsyncDal<TDbContext, TDomainObject, TIdent>(
    EfSession<TDbContext> session,
    IIdentityInfo<TDomainObject, TIdent> identityInfo) : EfAsyncDal<TDomainObject, TIdent>(session, session.NativeSession, identityInfo)
    where TDbContext : DbContext
    where TDomainObject : class
    where TIdent : notnull;
