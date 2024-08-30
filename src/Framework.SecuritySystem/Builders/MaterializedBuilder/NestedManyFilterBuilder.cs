using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class NestedManyFilterBuilder<TDomainObject, TNestedObject>(
    SecurityFilterBuilderFactory<TNestedObject> nestedBuilderFactory,
    SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath) : SecurityFilterBuilder<TDomainObject>
{
    private SecurityFilterBuilder<TNestedObject> NestedBuilder { get; } = nestedBuilderFactory.CreateBuilder(securityPath.NestedSecurityPath);

    public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission)
    {
        var nestedFilterExpression = this.NestedBuilder.GetSecurityFilterExpression(permission);

        var nestedCollectionFilterExpression = nestedFilterExpression.ToCollectionFilter();

        var mainCondition = securityPath.NestedObjectsPath.Select(v => nestedCollectionFilterExpression.Eval(v).Any()).InlineEval();

        switch (securityPath.Mode)
        {
            case ManySecurityPathMode.Any:
            {
                var emptyCondition = securityPath.NestedObjectsPath.Select(v => !v.Any());

                return emptyCondition.BuildOr(mainCondition);
            }

            case ManySecurityPathMode.AnyStrictly:

                return mainCondition;

            default:

                throw new ArgumentOutOfRangeException(nameof(securityPath));
        }
    }
}
