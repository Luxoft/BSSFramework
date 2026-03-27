using System.Linq.Expressions;

using Framework.Application.Lock;

namespace Framework.Application.DependencyInjection;

public interface IGenericNamedLockSetup
{
    IGenericNamedLockSetup SetNameLockType<TGenericNamedLock>(Expression<Func<TGenericNamedLock, string>> namePath)
        where TGenericNamedLock : new();

    IGenericNamedLockSetup AddContainer(Type containerType);

    IGenericNamedLockSetup AddManual(NamedLock namedLock);

    IGenericNamedLockSetup AddManual(Type domainType) => this.AddManual(new NamedLock(domainType));
}
