using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.Lock;

namespace Framework.DomainDriven;

public interface IDAL<TDomainObject, TIdent> : IAuditDAL<TDomainObject, TIdent>
{
    /// <summary>
    /// Предоставляет Queryable в контексте Read
    /// </summary>
    /// <returns></returns>
    IQueryable<TDomainObject> GetQueryable(LockRole lockRole, IFetchContainer<TDomainObject>? fetchContainer = null);

    TDomainObject GetById(TIdent id, LockRole lockRole);

    /// Return the persistent instance of the given entity class with the given identifier,
    /// assuming that the instance exists.
    TDomainObject Load(TIdent id);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    void Refresh(TDomainObject domainObject);

    void Lock(TDomainObject domainObject, LockRole lockRole);

    void Save(TDomainObject domainObject);

    void Insert(TDomainObject domainObject, TIdent id);

    void Remove(TDomainObject domainObject);
}
