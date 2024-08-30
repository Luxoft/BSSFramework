using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class AndFilterBuilder<TDomainObject>(
    SecurityFilterBuilderFactory<TDomainObject> builderFactory,
    SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    : BinaryFilterBuilder<TDomainObject, SecurityPath<TDomainObject>.AndSecurityPath>(builderFactory, securityPath)
{
    protected override Expression<Func<TArg1, TArg2, bool>> BuildOperation<TArg1, TArg2>(
        Expression<Func<TArg1, TArg2, bool>> arg1,
        Expression<Func<TArg1, TArg2, bool>> arg2) => arg1.BuildAnd(arg2);
}
