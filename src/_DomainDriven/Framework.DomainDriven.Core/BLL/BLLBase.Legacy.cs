using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public abstract partial class BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation>
    {
        [Obsolete("Use GetUnsecureQueryable", true)]
        public IQueryable<TDomainObject> GetQueryable()
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use GetSecureQueryable", true)]
        public IQueryable<TDomainObject> GetFilterQueryable(IFetchContainer<TDomainObject> fetchContainer = null)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use GetUnsecureQueryable", true)]
        public IQueryable<TDomainObject> GetQueryable(IFetchContainer<TDomainObject> fetchContainer)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use GetUnsecureQueryable", true)]
        public IQueryable<TDomainObject> GetQueryable(LockRole lockRole, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use GetSecureQueryable", true)]
        public IQueryable<TDomainObject> GetFilterQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use GetSecureQueryable", true)]
        public IQueryable<TDomainObject> GetFilterQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use GetUnsecureQueryable", true)]
        public IQueryable<TDomainObject> GetQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use GetUnsecureQueryable", true)]
        public IQueryable<TDomainObject> GetQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            return this.GetObjectsBy(filter, LockRole.None, fetchContainer);
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetObjectsBy(filter, new[] { firstFetch }.Concat(otherFetchs));
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetObjectsBy(filter, fetchs.ToFetchContainer());
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, LockRole lockRole, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            var result = ((IEnumerable<TDomainObject>)this.GetSecureQueryable(fetchContainer, lockRole).Where(filter)).Distinct().ToList();

            var queriedEventArgs = new ObjectsQueriedEventArgs<TDomainObject>(result, filter);

            this.SourceListener.InvokeObjectsQueried(queriedEventArgs);

            return queriedEventArgs.Result;
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, LockRole lockRole, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetObjectsBy(filter, lockRole, new[] { firstFetch }.Concat(otherFetchs));
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(Expression<Func<TDomainObject, bool>> filter, LockRole lockRole, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetObjectsBy(filter, lockRole, fetchs.ToFetchContainer());
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(IDomainObjectFilterModel<TDomainObject> filterModel, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (filterModel == null) throw new ArgumentNullException(nameof(filterModel));

            return this.GetObjectsBy(filterModel.ToFilterExpression(), fetchContainer);
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(IDomainObjectFilterModel<TDomainObject> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetObjectsBy(filter, new[] { firstFetch }.Concat(otherFetchs));
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListBy instead")]
        public IList<TDomainObject> GetObjectsBy(IDomainObjectFilterModel<TDomainObject> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetObjectsBy(filter, fetchs.ToFetchContainer());
        }
    }
}
