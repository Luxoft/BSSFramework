using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class AndFilterBuilder<TDomainObject>(
    SecurityFilterBuilderFactory<TDomainObject> builderFactory,
    SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    : BinaryFilterBuilder<TDomainObject, SecurityPath<TDomainObject>.AndSecurityPath>(builderFactory, securityPath)
{
    protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(
        Expression<Func<TArg, bool>> arg1,
        Expression<Func<TArg, bool>> arg2) => arg1.BuildAnd(arg2);
}
