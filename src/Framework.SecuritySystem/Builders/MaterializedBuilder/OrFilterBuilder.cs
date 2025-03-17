using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class OrFilterBuilder<TDomainObject>(
    SecurityFilterBuilderFactory<TDomainObject> builderFactory,
    SecurityPath<TDomainObject>.OrSecurityPath securityPath,
    IReadOnlyList<SecurityContextRestriction> securityContextRestrictions)
    : BinaryFilterBuilder<TDomainObject, SecurityPath<TDomainObject>.OrSecurityPath>(
        builderFactory,
        securityPath,
        securityContextRestrictions)
{
    protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(
        Expression<Func<TArg, bool>> arg1,
        Expression<Func<TArg, bool>> arg2) => arg1.BuildOr(arg2);
}
