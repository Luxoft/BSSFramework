using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class NestedManyFilterBuilder<TDomainObject, TNestedObject>(
    SecurityFilterBuilderFactory<TNestedObject> nestedBuilderFactory,
    SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath) : SecurityFilterBuilder<TDomainObject>
{
    private SecurityFilterBuilder<TNestedObject> NestedBuilder { get; } = nestedBuilderFactory.CreateBuilder(securityPath.NestedSecurityPath);

    public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
        HierarchicalExpandType expandType)
    {
        var baseFilter = this.NestedBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

        switch (securityPath.Mode)
        {
            case ManySecurityPathMode.Any:

                return (domainObject, permission) => !securityPath.NestedObjectsPath.Eval(domainObject).Any()

                                                     || securityPath.NestedObjectsPath.Eval(domainObject).Any(
                                                         nestedObject => baseFilter.Eval(nestedObject, permission));

            case ManySecurityPathMode.AnyStrictly:

                return (domainObject, permission) => securityPath.NestedObjectsPath.Eval(domainObject)
                                                         .Any(nestedObject => baseFilter.Eval(nestedObject, permission));

            default:

                throw new ArgumentOutOfRangeException("securityPath.Mode");
        }
    }
}
