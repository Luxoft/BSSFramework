using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public partial interface IBLLQueryBase<TDomainObject>
    {
        [Obsolete("Use GetSecureQueryable", true)]
        IQueryable<TDomainObject> GetFilterQueryable(IFetchContainer<TDomainObject> fetchContainer = null);

        [Obsolete("Use GetUnsecureQueryable", true)]
        IQueryable<TDomainObject> GetQueryable(IFetchContainer<TDomainObject> fetchContainer);

        [Obsolete("Use GetUnsecureQueryable", true)]
        IQueryable<TDomainObject> GetQueryable(LockRole lockRole, IFetchContainer<TDomainObject> fetchContainer = null);


        [Obsolete("Use GetSecureQueryable", true)]
        IQueryable<TDomainObject> GetFilterQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

        [Obsolete("Use GetSecureQueryable", true)]
        IQueryable<TDomainObject> GetFilterQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);

        [Obsolete("Use GetUnsecureQueryable", true)]
        IQueryable<TDomainObject> GetQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

        [Obsolete("Use GetUnsecureQueryable", true)]
        IQueryable<TDomainObject> GetQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(IDomainObjectFilterModel<TDomainObject> filter, IFetchContainer<TDomainObject> fetchContainer = null);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(IDomainObjectFilterModel<TDomainObject> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(IDomainObjectFilterModel<TDomainObject> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, IFetchContainer<TDomainObject> fetchContainer = null);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, LockRole lockRole, IFetchContainer<TDomainObject> fetchContainer = null);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, LockRole lockRole, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, LockRole lockRole, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);
    }
}
