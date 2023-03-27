using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;

using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public class NHibAsyncDal<TDomainObject, TIdent> : IAsyncDal<TDomainObject, TIdent>
        where TDomainObject : class, IIdentityObject<TIdent>
{
    private readonly INHibSession session;

    private readonly IExpressionVisitorContainer expressionVisitorContainer;

    public NHibAsyncDal(INHibSession session, IExpressionVisitorContainer expressionVisitorContainer)
    {
        this.session = session;

        this.expressionVisitorContainer = expressionVisitorContainer;
    }


    private ISession NativeSession => this.session.NativeSession;

    public IQueryable<TDomainObject> GetQueryable()
    {
        var queryable = this.NativeSession.Query<TDomainObject>();

        (queryable.Provider as VisitedQueryProvider)
                .FromMaybe("Register VisitedQueryProvider in Nhib configuration")
                .Visitor = this.expressionVisitorContainer.Visitor;

        return queryable;
    }

    public virtual TDomainObject Load(TIdent id) => this.NativeSession.Load<TDomainObject>(id);

    public virtual async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default) =>
        await this.NativeSession.LoadAsync<TDomainObject>(id, cancellationToken);

    public virtual async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        await this.NativeSession.SaveOrUpdateAsync(domainObject, cancellationToken);

        this.session.RegisterModified(domainObject, ModificationType.Save);
    }

    public virtual async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default)
    {
        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        await this.NativeSession.SaveAsync(domainObject, id, cancellationToken);

        this.session.RegisterModified(domainObject, ModificationType.Save);
    }

    public virtual async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        this.session.RegisterModified(domainObject, ModificationType.Remove);

        await this.NativeSession.DeleteAsync(domainObject, cancellationToken);
    }

    private void CheckWrite()
    {
        if (this.session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}
