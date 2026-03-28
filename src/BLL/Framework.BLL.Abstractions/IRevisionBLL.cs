using System.Linq.Expressions;

using Framework.BLL.Domain.DAL.Revisions;
using Framework.Core;


namespace Framework.BLL;

public interface IRevisionBLL<TDomainObject, TIdent>
{
    long GetCurrentRevision();

    long? GetPreviousRevision(TIdent id, long maxRevision);

    TDomainObject GetObjectByRevision(TIdent id, long revision);

    IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> idCollection, long revision);

    DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null);

    DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(TIdent id, Expression<Func<TDomainObject, TProperty>> propertyExpression, Period? period = null);

    DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(TIdent id, string propertyName, Period? period = null);
}
