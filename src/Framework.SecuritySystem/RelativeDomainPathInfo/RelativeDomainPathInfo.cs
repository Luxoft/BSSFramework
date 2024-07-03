using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public record RelativeDomainPathInfo<TFrom, TTo>(
    Expression<Func<TFrom, TTo>> Path) : IRelativeDomainPathInfo<TFrom, TTo>;
