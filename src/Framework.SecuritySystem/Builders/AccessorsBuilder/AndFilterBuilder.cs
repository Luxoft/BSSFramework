using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class AndFilterBuilder<TPermission, TDomainObject>(
    AccessorsFilterBuilderFactory<TPermission, TDomainObject> builderFactory,
    SecurityPath<TDomainObject>.AndSecurityPath securityPath,
    IReadOnlyList<SecurityContextRestriction> securityContextRestrictions)
    : BinaryFilterBuilder<TPermission, TDomainObject, SecurityPath<TDomainObject>.AndSecurityPath>(
        builderFactory,
        securityPath,
        securityContextRestrictions)
{
    protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(
        Expression<Func<TArg, bool>> arg1,
        Expression<Func<TArg, bool>> arg2) => arg1.BuildAnd(arg2);
}
