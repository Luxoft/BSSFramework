using System.Linq.Expressions;

namespace Framework.DomainDriven.Lock;

public record GenericNamedLockTypeInfo<TGenericNamedLock>(Expression<Func<TGenericNamedLock, string>> NamePath);
