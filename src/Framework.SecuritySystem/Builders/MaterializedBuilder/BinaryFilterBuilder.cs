using System.Linq.Expressions;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public abstract class BinaryFilterBuilder<TDomainObject, TSecurityPath>(
    SecurityFilterBuilderFactory<TDomainObject> builderFactory,
    TSecurityPath securityPath,IReadOnlyList<SecurityContextRestriction> securityContextRestrictions)
    : SecurityFilterBuilder<TDomainObject>
    where TSecurityPath : SecurityPath<TDomainObject>.BinarySecurityPath
{
    private SecurityFilterBuilder<TDomainObject> LeftBuilder { get; } = builderFactory.CreateBuilder(securityPath.Left, securityContextRestrictions);

    private SecurityFilterBuilder<TDomainObject> RightBuilder { get; } = builderFactory.CreateBuilder(securityPath.Right, securityContextRestrictions);

    protected abstract Expression<Func<TArg, bool>> BuildOperation<TArg>(
        Expression<Func<TArg, bool>> arg1,
        Expression<Func<TArg, bool>> arg2);

    public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission)
    {
        var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(permission);

        var rightFilter = this.RightBuilder.GetSecurityFilterExpression(permission);

        return this.BuildOperation(leftFilter, rightFilter);
    }
}
