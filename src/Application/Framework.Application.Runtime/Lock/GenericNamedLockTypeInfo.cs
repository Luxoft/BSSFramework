using System.Linq.Expressions;

namespace Framework.Application.Lock;

public record GenericNamedLockTypeInfo<TGenericNamedLock>(Expression<Func<TGenericNamedLock, string>> NamePath);
