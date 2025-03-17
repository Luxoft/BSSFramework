using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.ExpressionComparers;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public abstract record SecurityContextRestrictionFilterInfo
{
    public abstract Type SecurityContextType { get; }

    public abstract LambdaExpression GetBasePureFilter(IServiceProvider serviceProvider);
}

public abstract record SecurityContextRestrictionFilterInfo<TSecurityContext> : SecurityContextRestrictionFilterInfo
    where TSecurityContext : ISecurityContext
{
    public override Type SecurityContextType { get; } = typeof(TSecurityContext);

    public abstract Expression<Func<TSecurityContext, bool>> GetPureFilter(IServiceProvider serviceProvider);

    public override LambdaExpression GetBasePureFilter(IServiceProvider serviceProvider) => this.GetPureFilter(serviceProvider);
}

public record SecurityContextRestrictionFilterInfo<TSecurityContext, TService>(
    Expression<Func<TService, Expression<Func<TSecurityContext, bool>>>> Expression)
    : SecurityContextRestrictionFilterInfo<TSecurityContext>
    where TSecurityContext : ISecurityContext
    where TService : notnull
{
    public virtual bool Equals(SecurityContextRestrictionFilterInfo<TSecurityContext, TService>? other) =>
        ReferenceEquals(this, other)
        || (other is not null
            && ExpressionComparer.Value.Equals(this.Expression, other.Expression));


    public override Expression<Func<TSecurityContext, bool>> GetPureFilter(IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetRequiredService<TService>();

        return this.Expression.Eval(service);
    }

    public override int GetHashCode() => 0;
}
