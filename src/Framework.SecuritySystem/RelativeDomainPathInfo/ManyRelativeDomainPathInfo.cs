using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public record ManyRelativeDomainPathInfo<TFrom, TTo>(Expression<Func<TFrom, IEnumerable<TTo>>> Path) : IRelativeDomainPathInfo<TFrom, TTo>
{
    public IRelativeDomainPathInfo<TNewFrom, TTo> OverrideInput<TNewFrom>(Expression<Func<TNewFrom, TFrom>> selector)
    {
        return new ManyRelativeDomainPathInfo<TNewFrom, TTo>(this.Path.OverrideInput(selector));
    }

    public IRelativeDomainPathInfo<TFrom, TNewTo> Select<TNewTo>(Expression<Func<TTo, TNewTo>> selector)
    {
        return new ManyRelativeDomainPathInfo<TFrom, TNewTo>(
            this.Path.Select(items => items.Select(item => selector.Eval(item))).ExpandConst().InlineEval());
    }

    public IRelativeDomainPathInfo<TFrom, TNewTo> Select<TNewTo>(Expression<Func<TTo, IEnumerable<TNewTo>>> selector)
    {
        return new ManyRelativeDomainPathInfo<TFrom, TNewTo>(
            this.Path.Select(items => items.SelectMany(item => selector.Eval(item))).ExpandConst().InlineEval());
    }

    public Expression<Func<TFrom, bool>> CreateCondition(Expression<Func<TTo, bool>> filter)
    {
        return this.Path.Select(items => items.Any(item => filter.Eval(item))).ExpandConst().InlineEval();
    }

    public IEnumerable<TTo> GetRelativeObjects(TFrom source)
    {
        return this.Path.Eval(source);
    }
}
