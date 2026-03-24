using Framework.Application.Domain;
using Framework.BLL.OData;
using Framework.Core.Serialization;
using Framework.Events;
using Framework.OData;
using Framework.QueryLanguage;
using Framework.Validation;

using HierarchicalExpand;

namespace Framework.BLL.BLL.Default;

public abstract class DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent> :

        IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultBLLBaseContext&lt;TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer&gt;" /> class.
    /// </summary>
    /// <param name="serviceProvider">DI interface.</param>
    /// <param name="operationSender">The operation senders.</param>
    /// <param name="operationSender">The source listeners.</param>
    /// <param name="trackingService">The object state service.</param>
    /// <param name="standardExpressionBuilder">The standard expression builder.</param>
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
    /// standardExpressionBuilder
    /// or
    /// hierarchicalObjectExpanderFactory
    /// or
    /// fetchService
    /// or
    /// timeProvider</exception>
    protected DefaultBLLBaseContext(
            IServiceProvider serviceProvider,
            IEventOperationSender operationSender,
            IStandardExpressionBuilder standardExpressionBuilder,
            IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory)
    {
        this.ServiceProvider = serviceProvider;
        this.OperationSender = operationSender;

        this.StandardExpressionBuilder = standardExpressionBuilder ?? throw new ArgumentNullException(nameof(standardExpressionBuilder));
        this.HierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory ?? throw new ArgumentNullException(nameof(hierarchicalObjectExpanderFactory));

        this.SelectOperationParser = BusinessLogicSelectOperationParser.CachedDefault;
    }

    public IServiceProvider ServiceProvider { get; }

    public IEventOperationSender OperationSender { get; }

    public IStandardExpressionBuilder StandardExpressionBuilder { get; }

    public IParser<string, SelectOperation> SelectOperationParser { get; }

    public IHierarchicalObjectExpanderFactory HierarchicalObjectExpanderFactory { get; }

    protected abstract IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics { get; }

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
            IStandardExpressionBuilder standardExpressionBuilder,
            IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory)
            : base(serviceProvider, operationSender, standardExpressionBuilder, hierarchicalObjectExpanderFactory)
    {
    }

    public abstract TBLLFactoryContainer Logics { get; }

    protected override IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics => this.Logics;
}
