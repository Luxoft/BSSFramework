using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class NestedManyFilterBuilder<TDomainObject, TNestedObject>(
    SecurityFilterBuilderFactory<TNestedObject> nestedBuilderFactory,
    SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath,
    IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList) : SecurityFilterBuilder<TDomainObject>
{
    private SecurityFilterBuilder<TNestedObject> NestedBuilder { get; } = nestedBuilderFactory.CreateBuilder(securityPath.NestedSecurityPath, restrictionFilterInfoList);

    public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission)
    {
        var nestedFilterExpression = this.NestedBuilder.GetSecurityFilterExpression(permission);

        var nestedCollectionFilterExpression = nestedFilterExpression.ToCollectionFilter();

        var mainCondition = securityPath.NestedExpression.Select(v => nestedCollectionFilterExpression.Eval(v).Any()).InlineEval();

        switch (securityPath.Mode)
        {
            case ManySecurityPathMode.Any:
            {
                var emptyCondition = securityPath.NestedExpression.Select(v => !v.Any());

                return emptyCondition.BuildOr(mainCondition);
            }

            case ManySecurityPathMode.AnyStrictly:

                return mainCondition;

            default:

                throw new ArgumentOutOfRangeException(nameof(securityPath));
        }
    }
}
