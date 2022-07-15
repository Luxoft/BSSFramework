using System;
using System.ComponentModel;

using Framework.Core.Serialization;
using Framework.DomainDriven.BLL.Tracking;
using Framework.HierarchicalExpand;
using Framework.OData;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.QueryLanguage;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public abstract class DefaultBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent> :

            IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>,

            ITrackingServiceContainer<TPersistentDomainObjectBase>,

            IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBLLBaseContext&lt;TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer&gt;" /> class.
        /// </summary>
        /// <param name="serviceProvider">DI interface.</param>
        /// <param name="dalFactory">The dal factory.</param>
        /// <param name="operationListeners">The operation listeners.</param>
        /// <param name="sourceListeners">The source listeners.</param>
        /// <param name="objectStateService">The object state service.</param>
        /// <param name="standartExpressionBuilder">The standart expression builder.</param>
        /// <param name="validator">The validator.</param>
        /// <param name="hierarchicalObjectExpanderFactory">The hierarchical object expander factory.</param>
        /// <param name="fetchService">The fetch service.</param>
        /// <exception cref="ArgumentNullException">dalFactory
        /// or
        /// operationListeners
        /// or
        /// sourceListeners
        /// or
        /// objectStateService
        /// or
        /// standartExpressionBuilder
        /// or
        /// hierarchicalObjectExpanderFactory
        /// or
        /// fetchService
        /// or
        /// dateTimeService</exception>
        protected DefaultBLLBaseContext(
                [NotNull] IServiceProvider serviceProvider,
                [NotNull] IDALFactory<TPersistentDomainObjectBase, TIdent> dalFactory,
                [NotNull] BLLOperationEventListenerContainer<TDomainObjectBase> operationListeners,
                [NotNull] BLLSourceEventListenerContainer<TPersistentDomainObjectBase> sourceListeners,
                [NotNull] IObjectStateService objectStateService,
                [NotNull] IStandartExpressionBuilder standartExpressionBuilder,
                [NotNull] IValidator validator,
                [NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
                [NotNull] IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
        {
            if (objectStateService == null) throw new ArgumentNullException(nameof(objectStateService));

            this.ServiceProvider = serviceProvider;
            this.DalFactory = dalFactory ?? throw new ArgumentNullException(nameof(dalFactory));
            this.OperationListeners = operationListeners ?? throw new ArgumentNullException(nameof(operationListeners));
            this.SourceListeners = sourceListeners ?? throw new ArgumentNullException(nameof(sourceListeners));
            this.TrackingService = new TrackingService<TPersistentDomainObjectBase>(objectStateService);

            this.StandartExpressionBuilder = standartExpressionBuilder ?? throw new ArgumentNullException(nameof(standartExpressionBuilder));
            this.Validator = validator;
            this.HierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory ?? throw new ArgumentNullException(nameof(hierarchicalObjectExpanderFactory));

            this.SelectOperationParser = BusinessLogicSelectOperationParser.CachedDefault;

            this.FetchService = fetchService ?? throw new ArgumentNullException(nameof(fetchService));
        }

        public IValidator Validator { get; }

        public IFetchService<TPersistentDomainObjectBase, FetchBuildRule> FetchService { get; }

        public IServiceProvider ServiceProvider { get; }

        public IDALFactory<TPersistentDomainObjectBase, TIdent> DalFactory { get; }

        public ITrackingService<TPersistentDomainObjectBase> TrackingService { get; }

        public IBLLOperationEventListenerContainer<TDomainObjectBase> OperationListeners { get; }

        public BLLSourceEventListenerContainer<TPersistentDomainObjectBase> SourceListeners { get; }

        public IStandartExpressionBuilder StandartExpressionBuilder { get; }

        public IParser<string, SelectOperation> SelectOperationParser { get; }

        public IHierarchicalObjectExpanderFactory<TIdent> HierarchicalObjectExpanderFactory { get; }

        protected abstract IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics { get; }

        /// <inheritdoc />
        public virtual bool AllowVirtualPropertyInOdata(Type domainType)
        {
            return false;
        }

        /// <inheritdoc />
        public abstract bool AllowedExpandTreeParents<TDomainObject>()
            where TDomainObject : TPersistentDomainObjectBase;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IQueryableSource<TPersistentDomainObjectBase> GetQueryableSource()
        {
            return new BLLQueryableSource<IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>, TPersistentDomainObjectBase, TDomainObjectBase, TIdent>(this);
        }

        IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>.Logics => this.BaseLogics;
    }

    public abstract class DefaultBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer> : DefaultBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>,

        IBLLFactoryContainerContext<TBLLFactoryContainer>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
        where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
    {
        /// <inheritdoc />
        protected DefaultBLLBaseContext(
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] IDALFactory<TPersistentDomainObjectBase, TIdent> dalFactory,
            [NotNull] BLLOperationEventListenerContainer<TDomainObjectBase> operationListeners,
            [NotNull] BLLSourceEventListenerContainer<TPersistentDomainObjectBase> sourceListeners,
            [NotNull] IObjectStateService objectStateService,
            [NotNull] IStandartExpressionBuilder standartExpressionBuilder,
            [NotNull] IValidator validator,
            [NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
            [NotNull] IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
            : base(serviceProvider, dalFactory, operationListeners, sourceListeners, objectStateService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
        {
        }

        public abstract TBLLFactoryContainer Logics { get; }

        protected override IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics => this.Logics;
    }
}
