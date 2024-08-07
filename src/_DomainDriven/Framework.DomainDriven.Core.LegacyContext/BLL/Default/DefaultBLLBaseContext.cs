﻿using Framework.Core.Serialization;
using Framework.DomainDriven.Tracking;
using Framework.Events;
using Framework.HierarchicalExpand;
using Framework.OData;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.Validation;

namespace Framework.DomainDriven.BLL;

public abstract class DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent> :

        IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>,

        ITrackingServiceContainer<TPersistentDomainObjectBase>,

        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultBLLBaseContext&lt;TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer&gt;" /> class.
    /// </summary>
    /// <param name="serviceProvider">DI interface.</param>
    /// <param name="operationSender">The operation senders.</param>
    /// <param name="operationSender">The source listeners.</param>
    /// <param name="trackingService">The object state service.</param>
    /// <param name="standardExpressionBuilder">The standart expression builder.</param>
    /// <param name="validator">The validator.</param>
    /// <param name="hierarchicalObjectExpanderFactory">The hierarchical object expander factory.</param>
    /// <param name="fetchService">The fetch service.</param>
    /// <exception cref="ArgumentNullException">dalFactory
    /// or
    /// operationListeners
    /// or
    /// operationSender
    /// or
    /// objectStateService
    /// or
    /// standartExpressionBuilder
    /// or
    /// hierarchicalObjectExpanderFactory
    /// or
    /// fetchService
    /// or
    /// timeProvider</exception>
    protected DefaultBLLBaseContext(
            IServiceProvider serviceProvider,
            IEventOperationSender operationSender,
            ITrackingService<TPersistentDomainObjectBase> trackingService,
            IStandartExpressionBuilder standardExpressionBuilder,
            IValidator validator,
            IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
            IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
    {
        this.ServiceProvider = serviceProvider;
        this.OperationSender = operationSender;
        this.TrackingService = trackingService;

        this.StandartExpressionBuilder = standardExpressionBuilder ?? throw new ArgumentNullException(nameof(standardExpressionBuilder));
        this.Validator = validator;
        this.HierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory ?? throw new ArgumentNullException(nameof(hierarchicalObjectExpanderFactory));

        this.SelectOperationParser = BusinessLogicSelectOperationParser.CachedDefault;

        this.FetchService = fetchService ?? throw new ArgumentNullException(nameof(fetchService));
    }

    public IValidator Validator { get; }

    public IFetchService<TPersistentDomainObjectBase, FetchBuildRule> FetchService { get; }

    public IServiceProvider ServiceProvider { get; }

    public ITrackingService<TPersistentDomainObjectBase> TrackingService { get; }

    public IEventOperationSender OperationSender { get; }

    public IStandartExpressionBuilder StandartExpressionBuilder { get; }

    public IParser<string, SelectOperation> SelectOperationParser { get; }

    public IHierarchicalObjectExpanderFactory<TIdent> HierarchicalObjectExpanderFactory { get; }

    protected abstract IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics { get; }

    /// <inheritdoc />
    public virtual bool AllowVirtualPropertyInOdata(Type domainType)
    {
        return false;
    }

    IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>.Logics => this.BaseLogics;
}

public abstract class DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer> : DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent>,

    IBLLFactoryContainerContext<TBLLFactoryContainer>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
{
    /// <inheritdoc />
    protected DefaultBLLBaseContext(
            IServiceProvider serviceProvider,
            IEventOperationSender operationSender,
            ITrackingService<TPersistentDomainObjectBase> trackingService,
            IStandartExpressionBuilder standardExpressionBuilder,
            IValidator validator,
            IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
            IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
            : base(serviceProvider, operationSender, trackingService, standardExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
    {
    }

    public abstract TBLLFactoryContainer Logics { get; }

    protected override IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics => this.Logics;
}
