using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Exceptions;
using Framework.Persistent;

using JetBrains.Annotations;

using NHibernate.Linq;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.BLL
{
    public abstract partial class BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation> :

        OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TOperation>,

        IBLLBase<TBLLContext, TDomainObject>

        where TBLLContext : class, IBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>
        where TDomainObjectBase : class
        where TPersistentDomainObjectBase : class, TDomainObjectBase, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TOperation : struct, Enum
    {
        protected ISpecificationEvaluator SpecificationEvaluator { get; }

        private readonly Lazy<BLLSourceEventListener<TDomainObject>> _lazySourceListener;

        private readonly IDAL<TDomainObject, TIdent> _dal;


        protected BLLBase(TBLLContext context, ISpecificationEvaluator specificationEvaluator = null)
            : base(context)
        {
            this.SpecificationEvaluator = specificationEvaluator;
            this._dal = this.Context.DalFactory.CreateDAL<TDomainObject>();

            this._lazySourceListener = LazyHelper.Create(() => this.Context.SourceListeners.GetEventListener<TDomainObject>());
        }


        private BLLSourceEventListener<TDomainObject> SourceListener
        {
            get { return this._lazySourceListener.Value; }
        }

        #region Private.Method

        protected virtual IEnumerable<ExpressionVisitor> GetVisitors()
        {
            yield break;
        }

        private ExpressionVisitor GetQueryableExpressionVisitor()
        {
            return DefaultVisitorsHelper.Visitors
                                        .Concat(this.GetVisitors())
                                        .Concat(new[] { OptimizeWhereAndConcatVisitor.Value })
                                        .ToComposite();
        }

        protected virtual IQueryable<TDomainObject> ProcessSecurity(IQueryable<TDomainObject> queryable)
        {
            return queryable;
        }

        #endregion

        private void InternalSave(TDomainObject domainObject, TIdent id = default(TIdent))
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var savingEventArgs = new EventArgsWithCancel<TDomainObject>(domainObject);

            this.SourceListener.InvokeObjectSaving(savingEventArgs);

            if (!savingEventArgs.Cancel)
            {
                if (id.IsDefault())
                {
                    this._dal.Save(domainObject);
                }
                else
                {
                    this._dal.Insert(domainObject, id);
                }

                this.SourceListener.InvokeObjectSaved(new EventArgs<TDomainObject>(domainObject));
                base.Save(domainObject);
            }
        }

        public override void Save(TDomainObject domainObject)
        {
            this.InternalSave(domainObject);
        }

        public virtual void Insert(TDomainObject domainObject, TIdent id)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            if (id.IsDefault()) { throw new ArgumentOutOfRangeException(nameof(id)); }

            var isSimpleSave = !domainObject.Id.IsDefault() && this.GetUnsecureQueryable().Select(z => z.Id).Any(d => d.Equals(domainObject.Id));

            this.InternalSave(domainObject, isSimpleSave ? default(TIdent) : id);
        }

        public override void Remove(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var removingEventArgs = new EventArgsWithCancel<TDomainObject>(domainObject);

            this.SourceListener.InvokeObjectRemoving(removingEventArgs);

            if (!removingEventArgs.Cancel)
            {
                this._dal.Remove(domainObject);
                base.Remove(domainObject);
                this.SourceListener.InvokeObjectRemoved(removingEventArgs);
            }
        }

        public abstract OData.SelectOperationResult<TProjection> GetObjectsByOData<TProjection>(OData.SelectOperation<TProjection> selectOperation, Expression<Func<TDomainObject, TProjection>> projectionSelector);

        protected void SaveWithoutCascade([NotNull] TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var actions = CascadeActionHelper.Actions.ToArray(f => f(domainObject));

            actions.Foreach(pair => pair.Item1());

            try
            {
                this.InternalSave(domainObject);
            }
            finally
            {
                actions.Foreach(pair => pair.Item2());
            }
        }

        protected void InsertWithoutCascade([NotNull] TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.InsertWithoutCascade(domainObject, domainObject.Id);
        }

        protected void InsertWithoutCascade([NotNull] TDomainObject domainObject, TIdent id)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var actions = CascadeActionHelper.Actions.ToArray(f => f(domainObject));

            actions.Foreach(pair => pair.Item1());

            try
            {
                this.InternalSave(domainObject, id);
            }
            finally
            {
                actions.Foreach(pair => pair.Item2());
            }
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier,
        /// assuming that the instance exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TDomainObject Load(TIdent id) =>  this._dal.Load(id);

        public List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetListBy(filter, new[] { firstFetch }.Concat(otherFetchs));
        }

        public List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetListBy(filter, fetchs.ToFetchContainer());
        }

        public List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, IFetchContainer<TDomainObject> fetchContainer = null, LockRole lockRole = LockRole.None)
        {
            var result = ((IEnumerable<TDomainObject>)this.GetSecureQueryable(fetchContainer, lockRole).Where(filter)).Distinct().ToList();

            var queriedEventArgs = new ObjectsQueriedEventArgs<TDomainObject>(result, filter);

            this.SourceListener.InvokeObjectsQueried(queriedEventArgs);

            return queriedEventArgs.Result;
        }

        public List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, LockRole lockRole, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetListBy(filter, new[] { firstFetch }.Concat(otherFetchs));
        }

        public List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filterModel, IFetchContainer<TDomainObject> fetchContainer = null, LockRole lockRole = LockRole.None)
        {
            if (filterModel == null) throw new ArgumentNullException(nameof(filterModel));

            return this.GetListBy(filterModel.ToFilterExpression(), fetchContainer, lockRole);
        }

        public List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetListBy(filter, new[] { firstFetch }.Concat(otherFetchs));
        }

        public List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetListBy(filter, fetchs.ToFetchContainer());
        }

        public abstract OData.SelectOperationResult<TDomainObject> GetObjectsByOData(OData.SelectOperation selectOperation, IFetchContainer<TDomainObject> fetchContainer = null);

        public abstract OData.SelectOperationResult<TDomainObject> GetObjectsByOData(OData.SelectOperation<TDomainObject> selectOperation, IFetchContainer<TDomainObject> fetchContainer = null);

        public OData.SelectOperationResult<TDomainObject> GetObjectsByOData(OData.SelectOperation<TDomainObject> selectOperation, Expression<Func<TDomainObject, bool>> filter, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            var selectOperationWithFilter = selectOperation.AddFilter(filter);

            return this.GetObjectsByOData(selectOperationWithFilter, fetchContainer);
        }

        public OData.SelectOperationResult<TDomainObject> GetObjectsByOData(OData.SelectOperation<TDomainObject> selectOperation, IDomainObjectFilterModel<TDomainObject> filter, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            return this.GetObjectsByOData(selectOperation, filter.ToFilterExpression(), fetchContainer);
        }

        /// <summary>
        /// Получение IQueryable без учёта безопасности
        /// </summary>
        /// <param name="lockRole">Тип блокировки</param>
        /// <param name="fetchContainer">Подгружаемые свойства</param>
        /// <returns></returns>
        public IQueryable<TDomainObject> GetUnsecureQueryable(IFetchContainer<TDomainObject> fetchContainer, LockRole lockRole = LockRole.None)
        {
            return this._dal.GetQueryable(lockRole, fetchContainer, this.GetQueryableExpressionVisitor());
        }

        public IQueryable<TDomainObject> GetUnsecureQueryable(
            LockRole lockRole,
            IFetchContainer<TDomainObject> fetchContainer = null) => this.GetUnsecureQueryable(null, lockRole);

        /// <summary>
        /// Получение IQueryable без учёта безопасности
        /// </summary>
        /// <param name="firstFetch">Первое подгружаемое свойство</param>
        /// <param name="otherFetchs">Прочие подгружаемые свойства</param>
        /// <returns></returns>
        public IQueryable<TDomainObject> GetUnsecureQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetUnsecureQueryable(new[] { firstFetch }.Concat(otherFetchs));
        }

        /// <summary>
        /// Получение IQueryable без учёта безопасности
        /// </summary>
        /// <param name="fetchs">Подгружаемые свойства</param>
        /// <returns></returns>
        public IQueryable<TDomainObject> GetUnsecureQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetUnsecureQueryable(fetchs.ToFetchContainer());
        }

        /// <summary>
        /// Получение IQueryable с учётом безопасности
        /// </summary>
        /// <param name="lockRole">Тип блокировки</param>
        /// <param name="fetchContainer">Подгружаемые свойства</param>
        /// <returns></returns>
        public IQueryable<TDomainObject> GetSecureQueryable(IFetchContainer<TDomainObject> fetchContainer = null, LockRole lockRole = LockRole.None)
        {
            return this.ProcessSecurity(this.GetUnsecureQueryable(fetchContainer, lockRole));
        }

        /// <summary>
        /// Получение IQueryable с учётом безопасности
        /// </summary>
        /// <param name="firstFetch">Первое подгружаемое свойство</param>
        /// <param name="otherFetchs">Прочие подгружаемые свойства</param>
        /// <returns></returns>
        public IQueryable<TDomainObject> GetSecureQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetSecureQueryable(new[] { firstFetch }.Concat(otherFetchs));
        }

        /// <summary>
        /// Получение IQueryable с учётом безопасности
        /// </summary>
        /// <param name="fetchs">Подгружаемые свойства</param>
        /// <returns></returns>
        public IQueryable<TDomainObject> GetSecureQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetSecureQueryable(fetchs.ToFetchContainer());
        }

        protected IQueryable<TDomainObject> GetSecureQueryable([NotNull] IQueryableProcessor<TDomainObject> baseProcessor, IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (baseProcessor == null) throw new ArgumentNullException(nameof(baseProcessor));

            return this.GetSecureQueryable(fetchContainer).Pipe(q => baseProcessor.Process(q));
        }

        /// <summary>
        /// Выполняет поиск доменного объекта по условию.
        /// </summary>
        /// <param name="filter">Выражение, определяющее условие поиска.</param>
        /// <param name="getNotFoundException">Делегат, возвращающий исключение, которое необходимо бросить, если объект не найден.</param>
        /// <param name="fetchContainer">Пути, определяющие выгрузку графа.</param>
        /// <returns>Объект, найденный по условию.</returns>
        /// <exception cref="ArgumentNullException">Аргумент
        /// <paramref name="filter"/>
        /// or
        /// <paramref name="getNotFoundException"/>
        /// </exception>
        /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
        public TDomainObject GetObjectBy(
            Expression<Func<TDomainObject, bool>> filter,
            Func<Exception> getNotFoundException,
            IFetchContainer<TDomainObject> fetchContainer = null)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (getNotFoundException == null) throw new ArgumentNullException(nameof(getNotFoundException));

            return this.GetObjectBy(filter, false, fetchContainer).FromMaybe(getNotFoundException);
        }

        /// <summary>
        /// Выполняет поиск доменного объекта по условию.
        /// </summary>
        /// <param name="lockRole">Экземпляр <see cref="LockRole"/>, определяющий запрашиваемую блокировку.</param>
        /// <param name="filter">Выражение, определяющее условие поиска.</param>
        /// <param name="fetchContainer">Пути, определяющие выгрузку графа.</param>
        /// <param name="throwOnNotFound">
        ///   Когда передано <c>true</c> будет выброшено <see cref="BusinessLogicException"/>.
        ///   если объект по условию не найден.
        /// </param>
        /// <returns>Экземпляр найденного объекта или null, если объект не найден.</returns>
        /// <exception cref="BusinessLogicException">Если параметр throwOnNotFound задан как true и объект не найден.</exception>
        /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
        [Obsolete("Эта перегрузка метода будет удалёна в следующих версиях. Используйте вместо неё другие перегрузки.")]
        public TDomainObject GetObjectBy(
            LockRole lockRole,
            Expression<Func<TDomainObject, bool>> filter,
            IFetchContainer<TDomainObject> fetchContainer = null,
            bool throwOnNotFound = false)
        {
            return this.GetObjectBy(lockRole, filter, throwOnNotFound, fetchContainer);
        }

        /// <inheritdoc />
        public TDomainObject GetObjectBy(
            Expression<Func<TDomainObject, bool>> filter,
            bool throwOnNotFound = false,
            IFetchContainer<TDomainObject> fetchContainer = null)
        {
            return this.GetObjectBy(LockRole.None, filter, throwOnNotFound, fetchContainer);
        }

        /// <summary>
        /// Выполняет поиск доменного объекта по условию.
        /// </summary>
        /// <param name="lockRole">Экземпляр <see cref="LockRole"/>, определяющий запрашиваемую блокировку.</param>
        /// <param name="filter">Выражение, определяющее условие поиска.</param>
        /// <param name="throwOnNotFound">
        ///   Когда передано <c>true</c> будет выброшено <see cref="BusinessLogicException"/>.
        ///   если объект по условию не найден.
        /// </param>
        /// <param name="fetchContainer">Пути, определяющие выгрузку графа.</param>
        /// <returns>Экземпляр найденного объекта или null, если объект не найден.</returns>
        /// <exception cref="BusinessLogicException">Если параметр throwOnNotFound задан как true и объект не найден.</exception>
        /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
        public TDomainObject GetObjectBy(
            LockRole lockRole,
            Expression<Func<TDomainObject, bool>> filter,
            bool throwOnNotFound = false,
            IFetchContainer<TDomainObject> fetchContainer = null)
        {
            var result = this.GetListBy(filter, fetchContainer, lockRole).SingleOrDefault();

            if (null == result && throwOnNotFound)
            {
                throw new BusinessLogicException($"Object {typeof(TDomainObject).Name} not found by: {filter}.");
            }

            return result;
        }

        public TDomainObject GetObjectBy(Expression<Func<TDomainObject, bool>> filter, bool throwOnNotFound, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetObjectBy(filter, throwOnNotFound, new[] { firstFetch }.Concat(otherFetchs));
        }

        public TDomainObject GetObjectBy(Expression<Func<TDomainObject, bool>> filter, bool throwOnNotFound, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetObjectBy(filter, throwOnNotFound, fetchs.ToFetchContainer());
        }

        public List<TDomainObject> GetFullList(IFetchContainer<TDomainObject> fetchContainer = null)
        {
            return this.GetFullListAsync(fetchContainer).GetAwaiter().GetResult();
        }

        public virtual Task<List<TDomainObject>> GetFullListAsync(IFetchContainer<TDomainObject> fetchContainer = null, CancellationToken cancellationToken = default)
        {
            return this.GetSecureQueryable(fetchContainer).ToListAsync(cancellationToken);
        }

        public List<TDomainObject> GetFullList(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
        {
            if (firstFetch == null) throw new ArgumentNullException(nameof(firstFetch));
            if (otherFetchs == null) throw new ArgumentNullException(nameof(otherFetchs));

            return this.GetFullList(new[] { firstFetch }.Concat(otherFetchs));
        }

        public List<TDomainObject> GetFullList(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return this.GetFullList(fetchs.ToFetchContainer());
        }

        public void Lock(TDomainObject domainObject, LockRole lockRole)
        {
            this._dal.Lock(domainObject, lockRole);
        }

        #region Revision

        public virtual TDomainObject GetObjectByRevision(TIdent id, long revision)
        {
            return this._dal.GetObjectByRevision(id, revision);
        }

        public virtual long? GetPreviousRevision(TIdent id, long maxRevision)
        {
            return this._dal.GetPreviousRevision(id, maxRevision);
        }

        public virtual TDomainObject GetObjectsByPrevRevision(TIdent id)
        {
            var objectsRevisions = this.GetObjectRevisions(id).RevisionInfos.OrderByDescending(info => info.Date).ToList();

            var revision = objectsRevisions.First(() => new ArgumentException($"Object with id:{id} are not found. (Type:{typeof(TDomainObject).Name})")).RevisionNumber;

            return this.GetObjectByRevision(id, revision);
        }

        public virtual IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> idCollection, long revision)
        {
            return this._dal.GetObjectsByRevision(idCollection, revision);
        }

        public virtual DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null)
        {
            return this._dal.GetObjectRevisions(identity, period);
        }

        public virtual long? GetPreviusVersion(TIdent id, long maxRevisionNumber)
        {
            return this._dal.GetPreviousRevision(id, maxRevisionNumber);
        }

        public virtual IEnumerable<long> GetRevisions(TIdent id)
        {
            return this._dal.GetRevisions(id);
        }

        public virtual IList<Tuple<TDomainObject, long>> GetDomainObjectRevisions(TIdent id, int takeCount)
        {
            return this._dal.GetDomainObjectRevisions<TDomainObject>(id, takeCount);
        }

        public virtual DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(TIdent id,
            Expression<Func<TDomainObject, TProperty>> propertyExpression, Period? period = null)
        {
            return this._dal.GetPropertyRevisions(id, propertyExpression, period);
        }

        public virtual DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(TIdent id, string propertyName, Period? period = null)
        {
            return this._dal.GetPropertyRevisions<TProperty>(id, propertyName, period);
        }

        public virtual IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUnTypedPropertyChanges(TIdent id, string propertyName, Period? period = null)
        {
            return this._dal.GetUntypedPropertyRevisions(id, propertyName, period);
        }


        public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomainObject, bool>> expression)
        {
            return this._dal.GetIdentiesWithHistory(expression);
        }

        /// <summary>
        /// Get current revision. Actual info only after flush
        /// </summary>
        /// <returns></returns>
        public long GetCurrentRevision()
        {
            return this._dal.GetCurrentRevision();
        }

        #endregion

        private static class CascadeActionHelper
        {
            public static readonly ReadOnlyCollection<Func<TDomainObject, Tuple<Action, Action>>> Actions = typeof(TDomainObject)
                                                                                                           .GetDetailTypes()
                                                                                                           .Select(GetActions)
                                                                                                           .ToReadOnlyCollection();

            private static Func<TDomainObject, Tuple<Action, Action>> GetActions(Type detaiType)
            {
                return new Func<Func<IMaster<TDomainObject>, Tuple<Action, Action>>>(GetActions<IMaster<TDomainObject>, TDomainObject>)
                      .CreateGenericMethod(typeof(TDomainObject), detaiType)
                      .Invoke<Func<TDomainObject, Tuple<Action, Action>>>(null);
            }

            private static Func<TMaster, Tuple<Action, Action>> GetActions<TMaster, TDetail>()
                where TMaster : IMaster<TDetail>
            {
                return domainObject =>
                {
                    var details = domainObject.Details.ToArray();

                    return new Tuple<Action, Action>(() => domainObject.Details.Clear(), () => domainObject.Details.AddRange(details));
                };
            }
        }

        public IQueryable<TDomainObject> GetUnsecureQueryable() => this.GetUnsecureQueryable(null, LockRole.None);

#nullable enable

        public IQueryable<TProjection> GetQueryable<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetQuery(this.GetSecureQueryable(), specification);

        public TProjection? SingleOrDefault<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetQuery(this.GetSecureQueryable(), specification).SingleOrDefault();

        public TProjection Single<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetQuery(this.GetSecureQueryable(), specification).Single();

        public int Count<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetQuery(this.GetSecureQueryable(), specification).Count();

        public INuFutureEnumerable<TProjection> GetFuture<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetFuture(this.GetSecureQueryable(), specification);

        public INuFutureValue<TProjection> GetFutureValue<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetFutureValue(this.GetSecureQueryable(), specification);

        public INuFutureValue<int> GetFutureCount<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetFutureValue(this.GetSecureQueryable(), specification, x => x.Count());

        public IList<TProjection> GetList<TProjection>(Specification<TDomainObject, TProjection> specification) =>
                this.SpecificationEvaluator.GetQuery(this.GetSecureQueryable(), specification).ToList();

#nullable disable
    }
}
