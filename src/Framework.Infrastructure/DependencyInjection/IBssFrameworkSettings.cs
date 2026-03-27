using Framework.Application.DependencyInjection;
using Framework.Application.Events;
using Framework.Database.DALListener;
using Framework.Database.ExpressionVisitorContainer;

using SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public interface IBssFrameworkSettings : IBssFrameworkBuilderBase<IBssFrameworkSettings>
{
    bool RegisterDenormalizeHierarchicalDALListener { get; set; }

    IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemBuilder> setupAction);

    IBssFrameworkSettings AddNamedLocks(Action<IGenericNamedLockBuilder> setupAction);

    IBssFrameworkSettings AddListener<TListener>()
        where TListener : class, IDALListener;

    IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata;

    IBssFrameworkSettings AddQueryVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem;
}
