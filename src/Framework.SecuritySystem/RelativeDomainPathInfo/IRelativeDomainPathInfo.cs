using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public interface IRelativeDomainPathInfo<TFrom, TTo>
{
    IRelativeDomainPathInfo<TNewFrom, TTo> OverrideInput<TNewFrom>(Expression<Func<TNewFrom, TFrom>> selector);

    IRelativeDomainPathInfo<TFrom, TNewTo> Select<TNewTo>(Expression<Func<TTo, TNewTo>> selector);

    IRelativeDomainPathInfo<TFrom, TNewTo> Select<TNewTo>(Expression<Func<TTo, IEnumerable<TNewTo>>> selector);

    Expression<Func<TFrom, bool>> CreateCondition(Expression<Func<TTo, bool>> filter);

    IEnumerable<TTo> GetRelativeObjects(TFrom source);
}
