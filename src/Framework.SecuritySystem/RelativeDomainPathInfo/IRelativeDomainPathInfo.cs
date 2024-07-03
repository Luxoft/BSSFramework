using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public interface IRelativeDomainPathInfo<TFrom, TTo>
{
    Expression<Func<TFrom, TTo>> Path { get; }
}
