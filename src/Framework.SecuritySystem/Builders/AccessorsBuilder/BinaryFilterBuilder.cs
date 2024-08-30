using System.Linq.Expressions;

using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public abstract class BinaryFilterBuilder<TDomainObject, TSecurityPath>(
    AccessorsFilterBuilderFactory<TDomainObject> builderFactory,
    TSecurityPath securityPath)
    : AccessorsFilterBuilder<TDomainObject>
    where TSecurityPath : SecurityPath<TDomainObject>.BinarySecurityPath
{
    private AccessorsFilterBuilder<TDomainObject> LeftBuilder { get; } = builderFactory.CreateBuilder(securityPath.Left);

    private AccessorsFilterBuilder<TDomainObject> RightBuilder { get; } = builderFactory.CreateBuilder(securityPath.Right);

    protected abstract Expression<Func<TArg, bool>> BuildOperation<TArg>(
        Expression<Func<TArg, bool>> arg1,
        Expression<Func<TArg, bool>> arg2);

    public override Expression<Func<IPermission, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
    {
        var leftFilter = this.LeftBuilder.GetAccessorsFilter(domainObject, expandType);

        var rightFilter = this.RightBuilder.GetAccessorsFilter(domainObject, expandType);

        return this.BuildOperation(leftFilter, rightFilter);
    }
}
