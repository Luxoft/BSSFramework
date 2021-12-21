using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.OData;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL
{
    public partial interface IDefaultDomainBLLQueryBase<in TPersistentDomainObjectBase, TDomainObject, TIdent>
    {
        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        IList<TDomainObject> GetObjectsByIdents(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject> fetchContainer = null);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        IList<TDomainObject> GetObjectsByIdents(IEnumerable<TIdent> baseIdents, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        IList<TDomainObject> GetObjectsByIdents(IEnumerable<TIdent> baseIdents, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsUnsafe instead")]
        IList<TDomainObject> GetObjectsByIdentsUnsafe(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject> fetchContainer = null);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsUnsafe instead")]
        IList<TDomainObject> GetObjectsByIdentsUnsafe(IEnumerable<TIdent> baseIdents, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsUnsafe instead")]
        IList<TDomainObject> GetObjectsByIdentsUnsafe(IEnumerable<TIdent> baseIdents, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        IList<TDomainObject> GetObjectsByIdents<TIdentity>(IEnumerable<TIdentity> idents, IFetchContainer<TDomainObject> fetchContainer = null)
           where TIdentity : IIdentityObject<TIdent>;
    }
}
