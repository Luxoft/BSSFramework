using Framework.Application.DependencyInjection;
using Framework.Application.Events;
using Framework.Database.DALListener;
using Framework.Database.ExpressionVisitorContainer;

using SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public interface IBssFrameworkBuilder : IBssFrameworkBuilderBase<IBssFrameworkBuilder>
{
    bool RegisterDenormalizeHierarchicalDALListener { get; set; }

    IBssFrameworkBuilder AddSecuritySystem(Action<ISecuritySystemBuilder> setupAction);

    IBssFrameworkBuilder AddNamedLocks(Action<IGenericNamedLockBuilder> setupAction);

    IBssFrameworkBuilder AddListener<TListener>()
        where TListener : class, IDALListener;

    IBssFrameworkBuilder SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata;

    IBssFrameworkBuilder AddQueryVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem;
}
