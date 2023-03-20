using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.BLL;

public interface IRevisionBLL<TDomainObject, TIdent>
{
    long GetCurrentRevision();

    long? GetPreviousRevision(TIdent id, long maxRevision);

    TDomainObject GetObjectByRevision(TIdent id, long revision);

    DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null);

    DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(TIdent id, Expression<Func<TDomainObject, TProperty>> propertyExpression, Period? period = null);

    DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(TIdent id, string propertyName, Period? period = null);
}
