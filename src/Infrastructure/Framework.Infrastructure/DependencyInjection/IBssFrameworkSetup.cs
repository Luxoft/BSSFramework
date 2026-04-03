using Framework.Application.DependencyInjection;
using Framework.Application.Events;
using Framework.Database;
using Framework.Database.DALListener;
using Framework.Database.DependencyInjection;

using SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public interface IBssFrameworkSetup : IBssFrameworkSetup<IBssFrameworkSetup>
{
    bool RegisterDenormalizeHierarchicalDALListener { get; set; }

    IBssFrameworkSetup AddSecuritySystem(Action<ISecuritySystemSetup> setupAction);

    IBssFrameworkSetup AddNamedLocks(Action<IGenericNamedLockSetup> setupAction);

    IBssFrameworkSetup AddDatabase(Action<IDatabaseSetup> setupAction);

    IBssFrameworkSetup AddListener<TListener>()
        where TListener : class, IDALListener;

    IBssFrameworkSetup SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata;
}
