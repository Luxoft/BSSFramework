using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL
{
    public abstract partial class DefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation>
    {
        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        public IList<TDomainObject> GetObjectsByIdents<TIdentity>(IEnumerable<TIdentity> idents, IFetchContainer<TDomainObject> fetchContainer = null)
            where TIdentity : IIdentityObject<TIdent>
        {
            return this.GetObjectsByIdents(idents.Select(ident => ident.Id), fetchContainer);
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        public IList<TDomainObject> GetObjectsByIdents(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            var idents = baseIdents.ToList();

            var uniqueIdents = idents.Distinct().ToList();

            var uniqueResult = uniqueIdents.Split(MaxItemsInSql).SelectMany(path => this.GetObjectsBy(v => path.Contains(v.Id), fetchContainer)).ToList();

            if (uniqueResult.Count == uniqueIdents.Count)
            {
                var resultRequest = from ident in idents
                                    join domainObject in uniqueResult on ident equals domainObject.Id
                                    select domainObject;

                return resultRequest.ToList();
            }

            throw uniqueIdents.Except(uniqueResult.Select(v => v.Id)).Select(this.GetMissingObjectException).Aggregate();
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        public IList<TDomainObject> GetObjectsByIdents(
            IEnumerable<TIdent> baseIdents,
            Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch,
            params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            if (firstFetch == null)
            {
                throw new ArgumentNullException(nameof(firstFetch));
            }

            if (otherFetchs == null)
            {
                throw new ArgumentNullException(nameof(otherFetchs));
            }

            return this.GetObjectsByIdents(baseIdents, new[] { firstFetch }.Concat(otherFetchs));
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdents instead")]
        public IList<TDomainObject> GetObjectsByIdents(IEnumerable<TIdent> baseIdents, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            if (fetchs == null)
            {
                throw new ArgumentNullException(nameof(fetchs));
            }

            return this.GetObjectsByIdents(baseIdents, fetchs.ToFetchContainer());
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsUnsafe instead")]
        public IList<TDomainObject> GetObjectsByIdentsUnsafe(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            return baseIdents.Distinct().Split(MaxItemsInSql).SelectMany(path => this.GetObjectsBy(v => path.Contains(v.Id), fetchContainer)).ToList();
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsUnsafe instead")]
        public IList<TDomainObject> GetObjectsByIdentsUnsafe(
            IEnumerable<TIdent> baseIdents,
            Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch,
            params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            if (firstFetch == null)
            {
                throw new ArgumentNullException(nameof(firstFetch));
            }

            if (otherFetchs == null)
            {
                throw new ArgumentNullException(nameof(otherFetchs));
            }

            return this.GetObjectsByIdentsUnsafe(baseIdents, new[] { firstFetch }.Concat(otherFetchs));
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsUnsafe instead")]
        public IList<TDomainObject> GetObjectsByIdentsUnsafe(
            IEnumerable<TIdent> baseIdents,
            IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            if (fetchs == null)
            {
                throw new ArgumentNullException(nameof(fetchs));
            }

            return this.GetObjectsByIdentsUnsafe(baseIdents, fetchs.ToFetchContainer());
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsQueryable instead")]
        protected IList<TDomainObject> GetObjectsByIdentsQueryable(IQueryable<TIdent> baseIdents, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            var result = this.GetObjectsBy(v => baseIdents.Contains(v.Id), fetchContainer).ToList();

            return result;
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsNoSecurable instead")]
        protected IList<TProjection> GetObjectsByIdentsNoSecurable<TProjection>(IEnumerable<TIdent> baseIdents, Expression<Func<TDomainObject, TProjection>> projectionSelector, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (baseIdents == null) throw new ArgumentNullException(nameof(baseIdents));

            var idents = baseIdents.ToList();

            var uniqueIdents = idents.Distinct().ToList();

            var uniqueResult = uniqueIdents.Split(MaxItemsInSql).SelectMany(path => this.GetUnsecureQueryable(fetchContainer).Where(v => path.Contains(v.Id)).Select(projectionSelector)).ToList();

            if (uniqueResult.Count == uniqueIdents.Count)
            {
                var resultRequest = from ident in idents
                                    join projection in uniqueResult on ident equals ((IIdentityObject<TIdent>)projection).Id
                                    select projection;

                return resultRequest.ToList();
            }
            else
            {
                throw uniqueIdents.Except(uniqueResult.Select(v => ((IIdentityObject<TIdent>)v).Id)).Select(this.GetMissingObjectException).Aggregate();
            }
        }

        [Obsolete("Since v7.3 you should not use this method because of it's opacity. Use GetListByIdentsNoSecurable instead")]
        protected IList<TDomainObject> GetObjectsByIdentsNoSecurable(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (baseIdents == null)
            {
                throw new ArgumentNullException(nameof(baseIdents));
            }

            var idents = baseIdents.ToList();

            var uniqueIdents = idents.Distinct().ToList();

            var uniqueResult = uniqueIdents.Split(MaxItemsInSql).SelectMany(path => this.GetUnsecureQueryable(fetchContainer).Where(v => path.Contains(v.Id))).ToList();

            if (uniqueResult.Count == uniqueIdents.Count)
            {
                var resultRequest = from ident in idents
                                    join domainObject in uniqueResult on ident equals domainObject.Id
                                    select domainObject;

                return resultRequest.ToList();
            }

            throw uniqueIdents.Except(uniqueResult.Select(v => v.Id)).Select(this.GetMissingObjectException).Aggregate();
        }
    }
}
