using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Framework.Core;

namespace Framework.DomainDriven.DAL.Revisions;

public interface IAuditDAL<TDomainObject, TIdent>
{
    /// <summary>
    /// Gets the object by revision.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="revision">The revision number.</param>
    /// <returns></returns>
    TDomainObject GetObjectByRevision(TIdent id, long revision);

    /// <summary>
    /// Gets the object by revision.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="revision">The revision number.</param>
    /// <returns></returns>
    IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> id, long revision);

    IEnumerable<long> GetRevisions(TIdent id);

    IList<Tuple<T, long>> GetDomainObjectRevisions<T>(TIdent id, int takeCount) where T : class;

    /// <summary>
    /// Gets the revisions.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="maxRevision">Not include</param>
    /// <returns></returns>
    IEnumerable<long> GetRevisions(TIdent id, long maxRevision);

    /// <summary>
    /// Gets the previous revision.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="maxRevision">The max revision.</param>
    /// <returns></returns>
    long? GetPreviousRevision(TIdent id, long maxRevision);

    /// <summary>
    /// Gets the current revision.
    /// </summary>
    /// <returns></returns>
    long GetCurrentRevision();

    /*-------------------------------------*/

    /// <summary>
    /// Gets the property revisions.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="id">The id.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="period"></param>
    /// <returns></returns>
    DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(TIdent id, string propertyName, Period? period = null);

    /// <summary>
    /// Gets the properties revision.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="propertyExpression">The property expression.</param>
    /// <param name="period"> </param>
    /// <returns></returns>
    DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(TIdent id, Expression<Func<TDomainObject, TProperty>> propertyExpression, Period? period);

    /// <summary>
    /// Gets the untyped property revisions.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="period">The period.</param>
    /// <returns></returns>
    IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUntypedPropertyRevisions(TIdent id, string propertyName, Period? period = null);

    /// <summary>
    /// Gets the object revisions.
    /// </summary>
    /// <param name="identity">The identity.</param>
    /// <param name="period">The period.</param>
    /// <returns></returns>
    DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null);

    /// <summary>
    /// Gets the revisions objects by.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns></returns>
    IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomainObject, bool>> query);
}
