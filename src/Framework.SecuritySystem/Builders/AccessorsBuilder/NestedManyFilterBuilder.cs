using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class NestedManyFilterBuilder<TDomainObject, TNestedObject>(
    AccessorsFilterBuilderFactory<TNestedObject> nestedBuilderFactory,
    SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath) : AccessorsFilterBuilder<TDomainObject>
{
    private AccessorsFilterBuilder<TNestedObject> NestedBuilder { get; } = nestedBuilderFactory.CreateBuilder(securityPath.NestedSecurityPath);

    public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
        TDomainObject domainObject,
        HierarchicalExpandType expandType) =>
        securityPath.NestedObjectsPath.Eval(domainObject, LambdaCompileCache)
                    .BuildOr(item => this.NestedBuilder.GetAccessorsFilter(item, expandType));

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
