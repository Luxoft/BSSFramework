using Framework.Application.Domain;
using Framework.Application.Events;

using HierarchicalExpand;

namespace Framework.BLL.Default;

public abstract class DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent>(
    IServiceProvider serviceProvider,
    IEventOperationSender operationSender,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory)
    : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public IEventOperationSender OperationSender { get; } = operationSender;

    public IHierarchicalObjectExpanderFactory HierarchicalObjectExpanderFactory { get; } = hierarchicalObjectExpanderFactory;

    protected abstract IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics { get; }

    IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>.Logics => this.BaseLogics;
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
        IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory)
        : base(serviceProvider, operationSender, hierarchicalObjectExpanderFactory)
    {
    }

    public abstract TBLLFactoryContainer Logics { get; }

    protected override IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>> BaseLogics => this.Logics;
}
