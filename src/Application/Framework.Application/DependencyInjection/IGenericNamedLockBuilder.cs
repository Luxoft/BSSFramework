using System.Linq.Expressions;

using Framework.Application.Lock;

namespace Framework.Application.DependencyInjection;

public interface IGenericNamedLockBuilder
{
    IGenericNamedLockBuilder SetNameLockType<TGenericNamedLock>(Expression<Func<TGenericNamedLock, string>> namePath)
        where TGenericNamedLock : new();

    IGenericNamedLockBuilder AddContainer(Type containerType);

    IGenericNamedLockBuilder AddManual(NamedLock namedLock);

    IGenericNamedLockBuilder AddManual(Type domainType) => this.AddManual(new NamedLock(domainType));
}
