using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public abstract class BinaryFilterBuilder<TPermission, TDomainObject, TSecurityPath>(
    SecurityFilterBuilderFactory<TPermission, TDomainObject> builderFactory,
    TSecurityPath securityPath)
    : SecurityFilterBuilder<TPermission, TDomainObject>
    where TSecurityPath : SecurityPath<TDomainObject>.BinarySecurityPath
{
    private SecurityFilterBuilder<TPermission, TDomainObject> LeftBuilder { get; } = builderFactory.CreateBuilder(securityPath.Left);

    private SecurityFilterBuilder<TPermission, TDomainObject> RightBuilder { get; } = builderFactory.CreateBuilder(securityPath.Right);

    protected abstract Expression<Func<TArg1, TArg2, bool>> BuildOperation<TArg1, TArg2>(
        Expression<Func<TArg1, TArg2, bool>> arg1,
        Expression<Func<TArg1, TArg2, bool>> arg2);

    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
    {
        var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();
        var rightFilter = this.RightBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

        return this.BuildOperation(leftFilter, rightFilter);
    }
}
