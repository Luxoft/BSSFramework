using Anch.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework;

public class ComplexAsyncDal<TDomainObject, TIdent>(IServiceProvider serviceProvider, IDbContextTypeSource dbContextTypeSource)
    : IAsyncDal<TDomainObject, TIdent>

    where TDomainObject : class
    where TIdent : notnull
{
    private readonly Lazy<IAsyncDal<TDomainObject, TIdent>> lazyInnerAsyncDal = LazyHelper.Create(() =>
    {
        var dbContextType = dbContextTypeSource.GetDbContextType(typeof(TDomainObject));

        return dbContextType == dbContextTypeSource.PrimaryDbContextType
                   ? (IAsyncDal<TDomainObject, TIdent>)serviceProvider.GetRequiredKeyedService<IAsyncDal<TDomainObject, TIdent>>(IDbContextTypeSource.PrimaryKey)
                   : (IAsyncDal<TDomainObject, TIdent>)serviceProvider.GetRequiredService(
                       typeof(SecondaryEfAsyncDal<,,>).MakeGenericType(dbContextType, typeof(TDomainObject), typeof(TIdent)));
    });

    private IAsyncDal<TDomainObject, TIdent> InnerAsyncDal => this.lazyInnerAsyncDal.Value;

    public IQueryable<TDomainObject> GetQueryable() => this.InnerAsyncDal.GetQueryable();

    public TDomainObject Load(TIdent id) => this.InnerAsyncDal.Load(id);

    public Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct) => this.InnerAsyncDal.LoadAsync(id, ct);

    public Task RefreshAsync(TDomainObject domainObject, CancellationToken ct) => this.InnerAsyncDal.RefreshAsync(domainObject, ct);

    public Task SaveAsync(TDomainObject domainObject, CancellationToken ct) => this.InnerAsyncDal.SaveAsync(domainObject, ct);

    public Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct) => this.InnerAsyncDal.InsertAsync(domainObject, id, ct);

    public Task RemoveAsync(TDomainObject domainObject, CancellationToken ct) => this.InnerAsyncDal.RemoveAsync(domainObject, ct);

    public Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct) => this.InnerAsyncDal.LockAsync(domainObject, lockRole, ct);
}
