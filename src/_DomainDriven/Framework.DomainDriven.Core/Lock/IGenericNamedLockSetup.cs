using System.Linq.Expressions;

namespace Framework.DomainDriven.Lock;

public interface IGenericNamedLockSetup
{
    IGenericNamedLockSetup SetNameLockType<TGenericNamedLock>(Expression<Func<TGenericNamedLock, string>> namePath)
        where TGenericNamedLock : new();

    IGenericNamedLockSetup AddContainer(Type containerType);

    IGenericNamedLockSetup AddManual(NamedLock namedLock);

    IGenericNamedLockSetup AddManual(Type domainType) => this.AddManual(new NamedLock(domainType));
}
